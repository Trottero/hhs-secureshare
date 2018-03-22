using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SecureShare.Website.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.ProjectOxford.Face;
using SecureShare.Website.Models;

namespace SecureShare.Website.Controllers
{
    public class FaceController : Controller
    {
        private readonly FileReader _fr;
        private readonly IHostingEnvironment _environment;

        public FaceController(FileReader fr, IHostingEnvironment environment)
        {
            _fr = fr;
            _environment = environment;
        }

        public IActionResult Index()
        {
            ViewData["Result"] = " ";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authentication()
        {
            string capturedImage;
            using (var reader = new System.IO.StreamReader(HttpContext.Request.Body, System.Text.Encoding.UTF8))
            {
                string hexString = reader.ReadToEnd();
                string imageName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
                capturedImage = Path.Combine(_environment.WebRootPath, $"CamPics/{imageName}.png");
                System.IO.File.WriteAllBytes(capturedImage, ConvertHexToBytes(hexString));
            }
            //The second parameter should be removed for the user name.
            //When you are testing the application. Please Change "Henk" to something else.
            var result = await _fr.Authenticate(capturedImage, "Henk");
            
            ViewData["Result"] = result.PersonVerifyResult;

            return Ok();
        }

        [HttpPost]
        public IActionResult AuthenticationHtml5(string Source)
        {
            //The new way for the webcam with HTML5:
            //https://www.html5rocks.com/en/tutorials/getusermedia/intro/
            string imageName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
            string capturedImage = Path.Combine(_environment.WebRootPath, $"CamPics/{imageName}.png");


            Source = Source.Substring(Source.IndexOf(",") + 1);
            byte[] data = Convert.FromBase64String(Source);

            System.IO.File.WriteAllBytes(capturedImage, data);

            //The second parameter should be removed for the user name.
            //When you are testing the application. Please Change "Henk" to something else.

            Task task = _fr.Authenticate(capturedImage, "Henk");
            while (true)
            {
                if (task.IsCompleted)
                {
                    ViewData["Result"] = _fr.ra.PersonVerifyResult;
                    return View();
                }
                Task.Delay(1000);
            }
        }

        private static byte[] ConvertHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}