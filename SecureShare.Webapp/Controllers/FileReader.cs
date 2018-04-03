using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Face;
using SecureShare.Webapp.Extensions;
using SecureShare.Website.Models;

namespace SecureShare.Website.Controllers
{
    public class FileReader
    {
        private readonly FaceApiCoding _options;
        private readonly FaceServiceClient _faceServiceClient;
        public FileReader(IOptions<FaceApiCoding> optionsAccessor)
        {
            _options = optionsAccessor.Value;
            _faceServiceClient = new FaceServiceClient(_options.SubscriptionKey, _options.UriBase);
        }

        private async Task RecreateGroup()
        {
            await _faceServiceClient.DeletePersonGroupAsync(_options.PersonGroupId);
            await _faceServiceClient.CreatePersonGroupAsync(_options.PersonGroupId, "SecureShare");
        }

        public async Task<ResultAuth> Authenticate(string pathToUser, string userName)
        {
            //await RecreateGroup();
            Stream s = File.OpenRead(pathToUser);
            //If there is no face on the image an error wil appear.
            //If there are more faces on one image throw an error.
            var faces = await _faceServiceClient.DetectAsync(s);
            s.Dispose();
            s.Close();

            if (faces.Count() == 0)
                throw new FaceAuthenticationException("There are no recognizable faces on the image.");
            if (faces.Count() > 1)
                throw new FaceAuthenticationException("There are too many faces on the image.");
            var faceIds = faces.Select(face => face.FaceId).ToArray().First();

            Guid pId;
            var personList = await _faceServiceClient.GetPersonsAsync(_options.PersonGroupId);
            var p = personList.SingleOrDefault(n => n.Name == userName);

            if (p == null)
            {
                var person = await _faceServiceClient.CreatePersonAsync(_options.PersonGroupId, userName);
                pId = person.PersonId;
                await AddPersonFaceAsync(pathToUser, pId);

                return new ResultAuth(1);
            }
            pId = p.PersonId;

            var res = await _faceServiceClient.VerifyAsync(
                Guid.Parse(faceIds.ToString()), //The image that will be compared with the facegroup images.
                 _options.PersonGroupId,        //ID of the person.
                Guid.Parse(pId.ToString())      //GroupId where the person is in.
                );     
            // Verification result contains IsIdentical (true or false) and Confidence (in range 0.0 ~ 1.0),
            // here we update verify result on UI by PersonVerifyResult binding

            if (res.Confidence > 0.7)
            {
                await AddPersonFaceAsync(pathToUser, pId);
                await Task.Delay(500);
                await _faceServiceClient.TrainPersonGroupAsync(_options.PersonGroupId);
                await Task.Delay(500);
                await ImageControle(userName);
            }

            return new ResultAuth(res.Confidence);
        }

        private async Task AddPersonFaceAsync(string path, Guid pId)
        {
            Stream s2 = File.OpenRead(path);
            try
            {
                await _faceServiceClient.AddPersonFaceAsync(_options.PersonGroupId, pId, s2);
                await _faceServiceClient.TrainPersonGroupAsync(_options.PersonGroupId);
                s2.Dispose();
                s2.Close();
            }
            catch (Exception)
            {
                throw new FaceAuthenticationException("Something went wrong with getting the image again.");
            }
        }

        private async Task ImageControle(string userName)
        {
            var personList = await _faceServiceClient.GetPersonsAsync(_options.PersonGroupId);
            var p = personList.SingleOrDefault(n => n.Name == userName);
            try
            {
                if (p.PersistedFaceIds.Length > 5)
                {
                    var first = p.PersistedFaceIds[0];
                    await _faceServiceClient.DeletePersonFaceAsync(
                        _options.PersonGroupId,
                        p.PersonId,
                        first);
                    await _faceServiceClient.TrainPersonGroupAsync(_options.PersonGroupId);
                }
            }
            catch (NullReferenceException nullExc)
            {

            }
        }

        private async Task DeleteAllFacesFromUser(Guid userGuid)
        {
            await _faceServiceClient.DeleteFaceListAsync(userGuid.ToString());
        }
    }
}
