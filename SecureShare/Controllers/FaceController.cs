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
            return View();
        }

        [HttpPost]
        public string Capture(string time)
        {
            string capturedImage = Path.Combine(_environment.WebRootPath, $"{time}.jpg");

            using (var reader = new System.IO.StreamReader(HttpContext.Request.Body, System.Text.Encoding.UTF8))
            {
                string hexString = reader.ReadToEnd();
                hexString = hexString.Substring(hexString.IndexOf(',') + 1);
                byte[] data = Convert.FromBase64String(hexString);

                System.IO.File.WriteAllBytes(capturedImage, data);
            }

            return capturedImage;
        }
        
        [HttpPost]
        public async Task<IActionResult> Result(string capturedImage)
        {
            //The second parameter should be removed for the user name.
            //When you are testing the application. Please Change "Henk" to something else.
            try
            {
                var resultAuth = await _fr.Authenticate(capturedImage, "Henk");
                System.IO.File.Delete(capturedImage);
                ViewData["error"] = " ";
                return View(resultAuth);
            }
            catch (Exception codeException)
            {
                System.IO.File.Delete(capturedImage);
                if (codeException.Message.Equals("1")){
                    ViewData["error"] = "There are no recognizable on the image.";
                } else if (codeException.Message.Equals("2")){
                    ViewData["error"] = "There are too many recognizable on the image.";
                } else if (codeException.Message.Equals("3")){
                    ViewData["error"] = "Something went wrong with getting the image again.";
                }else if(codeException.Message.StartsWith("Could not find file")){
                    ViewData["error"] = "The image is already deleted.";
                }
                return View(new ResultAuth(0));
            }
        }
    }
}