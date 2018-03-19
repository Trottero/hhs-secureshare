using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SecureShare.Website.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.ProjectOxford.Face;

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