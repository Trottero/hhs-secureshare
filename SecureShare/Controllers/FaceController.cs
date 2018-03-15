using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SecureShare.Website.ViewModels;
using System;
using System.IO;

namespace SecureShare.Website.Controllers
{
    public class FaceController : Controller
    {
        private readonly FileReader _fr;

        public FaceController(FileReader fr)
        {
            _fr = fr;
        }

        public IActionResult Index()
        {
            ViewData["CapturedImage"] = @"C:\Users\Jordi\Documents\School\SE-11\ProjectExtra\Jordi5.jpg";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(FaceData fd)
        {
            /*
            string CapturedImage;
            if (Request.InputStream.Length > 0)
            {
                using (StreamReader reader = new StreamReader(Request.InputStream))
                {
                    string hexString = Server.UrlEncode(reader.ReadToEnd());
                    string imageName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
                    string imagePath = string.Format("~/Captures/{0}.png", imageName);
                    System.IO.File.WriteAllBytes(Server.MapPath(imagePath), ConvertHexToBytes(hexString));
                    CapturedImage = VirtualPathUtility.ToAbsolute(imagePath);
                }
            }
            */

            string PathToDir = @"C:\Users\Jordi\Documents\School\SE-11\ProjectExtra\Facegroup\Jordi";
            string imageP = fd.PathToCamPic;
            var result = await _fr.Authenticate(PathToDir, imageP, "Jordi");
            
            ViewData["Result"] = result.PersonVerifyResult;
            return View();
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