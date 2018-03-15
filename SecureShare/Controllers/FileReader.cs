using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Options;
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

        public async Task<ResultAuth> Authenticate(string pathToDir, string pathToUser, string userName)
        {
            var faceServiceClient = new FaceServiceClient(_options.SubscriptionKey, _options.UriBase);
            var person = await faceServiceClient.CreatePersonAsync(_options.PersonGroupId, userName);

            foreach (string imagePath in Directory.GetFiles(pathToDir, "*.jpg")){
                using (Stream s = File.OpenRead(imagePath))
                    await faceServiceClient.AddPersonFaceAsync(_options.PersonGroupId, person.PersonId, s);
            }

            await faceServiceClient.TrainPersonGroupAsync(_options.PersonGroupId);

            using (Stream s = File.OpenRead(pathToUser))
            {
                var faces = await faceServiceClient.DetectAsync(s);
                var faceIds = faces.Select(face => face.FaceId).ToArray().First();

                string personId = "";
                var personList = await faceServiceClient.GetPersonsAsync(_options.PersonGroupId);

                foreach (var p in personList){ //Search for the user in the PersonGroup
                    if (p.Name == userName)
                        personId = p.PersonId.ToString();
                }

                var res = await faceServiceClient.VerifyAsync(
                    Guid.Parse(faceIds.ToString()),     //The image that will be compared with the facegroup images.
                    Guid.Parse(personId),               //ID of the person.
                    personGroupId: _options.PersonGroupId);      //GroupId where the person is in.

                // Verification result contains IsIdentical (true or false) and Confidence (in range 0.0 ~ 1.0),
                // here we update verify result on UI by PersonVerifyResult binding

                return new ResultAuth(res.Confidence);
            }
        }
    }
}
