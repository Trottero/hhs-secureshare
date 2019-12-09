using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SecureShare.Webapp.Data;
using SecureShare.Webapp.Extensions;
using SecureShare.Website.Models;

namespace SecureShare.Website.Controllers
{
    public class FaceController : Controller
    {
        private readonly FileReader _fr;
        private readonly IHostingEnvironment _environment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public FaceController(FileReader fr, IHostingEnvironment environment, SignInManager<ApplicationUser> signInManager
            , UserManager<ApplicationUser> userManager)
        {
            _fr = fr;
            _environment = environment;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index(string returnUrl, string errorMessage)
        {
            ViewData["error"] = errorMessage;
            return View(returnUrl);
        }

        [HttpPost]
        public string Capture()
        {
            //Make a name for the new image and a path to the new image
            string imageName = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
            string capturedImage = Path.Combine(Path.GetTempPath(), $"{imageName}.jpg");

            using (var reader = new StreamReader(HttpContext.Request.Body, System.Text.Encoding.UTF8))
            {   //Get the stream of data from the webcam and create a JPG from it to the path made above
                string hexString = reader.ReadToEnd();
                hexString = hexString.Substring(hexString.IndexOf(',') + 1);
                byte[] data = Convert.FromBase64String(hexString);

                System.IO.File.WriteAllBytes(capturedImage, data);
            }

            return capturedImage;
        }

        public async Task<IActionResult> Result(string capturedImage, string returnUrl)
        {
            var errorMessage = "";
            try
            {   //Get the current user ID.
                var info = await _signInManager.GetExternalLoginInfoAsync();
                var userId = info.Principal.Claims.Single(e =>
                    e.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;
                
                //Sent the path of the image and the userId for authentication.
                //var resultAuth = await _fr.Authenticate(capturedImage, userId);
                //System.IO.File.Delete(capturedImage);
                //if (resultAuth.IsPerson)
                //{
                    return RedirectToPage("/Account/ExternalLogin", "Finalize", new {returnUrl = returnUrl});
                //}

                throw new FaceAuthenticationException(
                    "Sorry, the face captured does not correspond to the saved faces for this account.");

            }
            catch (FaceAuthenticationException codeException)
            {
                errorMessage = codeException.Message;

            }
            catch (FileNotFoundException exception)
            {
                errorMessage =
                    "Something went wrong whilst deleting the temporary file on the server. If this issue persists please contact a system administrator.";
            }
            catch (Exception ex)
            {
                errorMessage =
                    "Please contact your system administrator if this issue persists. Give them the following code for reference: " +
                    ex.Message;
            }
            return RedirectToAction("Index", "Face",
                new
                {
                    returnUrl = returnUrl,
                    errorMessage = errorMessage
                });
        }

        [HttpPost("deleteimages")]
        public async Task<IActionResult> DeleteImages()
        {
            var user = await _userManager.GetUserAsync(User);
            var userlogins = await _userManager.GetLoginsAsync(user);
            var providerKey = userlogins.First().ProviderKey;

            var message = "Your images have been succesfully deleted";
            try
            {
                await _fr.DeleteAllFacesFromUser(providerKey);
            }
            catch (FaceAuthenticationException ex)
            {
                message = ex.Message;
            }
            this.RouteData.Values.Clear();
            return RedirectToPage("/Account/Manage/Index", new { message = message});
        }
    }
}