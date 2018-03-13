using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace SecureShare.Website.Controllers
{
    public class FileReader
    {
        const string subscriptionKey = "71f6ecc1f2464f91b46c1f44b2f0e24f";
        const string uriBase = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/";
        const string personGroupId = "testing";
        FaceServiceClient faceServiceClient = new FaceServiceClient(subscriptionKey, uriBase);
        public string PersonVerifyResult = "Het werkte niet ;(";
        public Boolean isIdentical = false;


        private async Task MakeFullGroupPerson(string pathToDir, string userName)
        {
            CreatePersonResult friend = await faceServiceClient.CreatePersonAsync(personGroupId, userName);
            
            foreach (string imagePath in Directory.GetFiles(pathToDir, "*.jpg"))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    await faceServiceClient.AddPersonFaceAsync(personGroupId, friend.PersonId, s);
                }
            }

            await faceServiceClient.TrainPersonGroupAsync(personGroupId);
        }

        private async Task Authenticate(string pathToUser, string userName)
        {
            using (Stream s = File.OpenRead(pathToUser))
            {
                var faces = await faceServiceClient.DetectAsync(s);
                var faceIds = faces.Select(face => face.FaceId).ToArray().First();

                string personId = "";
                var personList = await faceServiceClient.GetPersonsAsync(personGroupId);

                foreach (Person p in personList)
                {
                    if (p.Name == userName)
                    {
                        personId = p.PersonId.ToString();
                    }
                }

                var res = await faceServiceClient.VerifyAsync(
                    Guid.Parse(faceIds.ToString()),     //The image that will be compared with the facegroup images.
                    Guid.Parse(personId),               //ID of the person.
                    personGroupId: personGroupId);      //GroupId where the person is in.

                // Verification result contains IsIdentical (true or false) and Confidence (in range 0.0 ~ 1.0),
                // here we update verify result on UI by PersonVerifyResult binding

                isIdentical = res.IsIdentical;
                PersonVerifyResult = string.Format("{0} ({1:0.0})", res.IsIdentical ? "The face belongs to the person" : "The face not belong to the person", res.Confidence);
            }
        }
        
        public async Task<string> resultAsync(string pathToDir, string pathToUser, string userName)
        {
            await MakeFullGroupPerson(pathToDir, userName);
            await Authenticate(pathToUser, userName);
            return PersonVerifyResult;
        }
    }
}
