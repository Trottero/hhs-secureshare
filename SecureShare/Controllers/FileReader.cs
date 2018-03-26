using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Face;
using SecureShare.Website.Models;

namespace SecureShare.Website.Controllers
{
    public class FileReader
    {
        private readonly FaceApiCoding _options;
        public FileReader(IOptions<FaceApiCoding> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public async Task<ResultAuth> Authenticate(string pathToUser, string userName)
        {
            var faceServiceClient = new FaceServiceClient(_options.SubscriptionKey, _options.UriBase);

            Stream s = File.OpenRead(pathToUser);
            //If there is no face on the image an error wil appear.
            //If there are more faces on one image throw an error.
            var faces = await faceServiceClient.DetectAsync(s);
            s.Dispose();
            s.Close();

            if (faces.Count() == 0)
                throw new Exception("No face on the image");
            if (faces.Count() > 1)
                throw new Exception("Too many faces on the image");
            var faceIds = faces.Select(face => face.FaceId).ToArray().First();

            Guid pId;
            bool found = false;
            var personList = await faceServiceClient.GetPersonsAsync(_options.PersonGroupId);
                
            foreach (var p in personList){ //Search for the user in the PersonGroup
                if (p.Name == userName)
                {
                    pId = p.PersonId;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Stream s2 = File.OpenRead(pathToUser);
                var person = await faceServiceClient.CreatePersonAsync(_options.PersonGroupId, userName);
                pId = person.PersonId;
                await faceServiceClient.AddPersonFaceAsync(_options.PersonGroupId, pId, s2);
                await faceServiceClient.TrainPersonGroupAsync(_options.PersonGroupId);
                s2.Dispose();
                s2.Close();
                return new ResultAuth(1);
            }

            var res = await faceServiceClient.VerifyAsync(
                Guid.Parse(faceIds.ToString()),             //The image that will be compared with the facegroup images.
                Guid.Parse(pId.ToString()),                 //ID of the person.
                personGroupId: _options.PersonGroupId);     //GroupId where the person is in.
            // Verification result contains IsIdentical (true or false) and Confidence (in range 0.0 ~ 1.0),
            // here we update verify result on UI by PersonVerifyResult binding

            if (res.Confidence > 0.5)
            {
                Stream s2 = File.OpenRead(pathToUser);
                await faceServiceClient.AddPersonFaceAsync(_options.PersonGroupId, pId, s2);
                await faceServiceClient.TrainPersonGroupAsync(_options.PersonGroupId);
                s2.Dispose();
                s2.Close();
            }
            return new ResultAuth(res.Confidence);
        }
    }
}
