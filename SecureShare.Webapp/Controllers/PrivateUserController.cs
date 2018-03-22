using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;

namespace SecureShare.Website.Controllers
{
    [Route("user")]
    [Authorize]
    public class PrivateUserController : Controller
    {
        private readonly IUserFileService _userFileService;
        private readonly IUserService _userService;
        private readonly string _nameIdentifierSchemaLocation = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public PrivateUserController(IUserFileService userFileService, IUserService userService)
        {
            _userFileService = userFileService;
            _userService = userService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var id = new Guid(User.Claims
                .Single(e => e.Type.Equals(_nameIdentifierSchemaLocation)).Value);
            var files = await _userFileService.GetFilesFromUser(id);
            return View(files);
        }

        [HttpGet("upload")]
        public async Task<IActionResult> UploadAFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var userfile = await _userFileService.AddUserFileAsync(file, new Guid(User.Claims.Single(e => e.Type.Equals(_nameIdentifierSchemaLocation)).Value));
            return RedirectToAction("Dashboard");
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var userFile = await _userFileService.GetUserFileAsync(id);
            var downloadInfo = await _userFileService.GetUserFileDownloadPath(userFile);
            return GetFileReadyToDownload(downloadInfo.RootPath, downloadInfo.FileName, downloadInfo.FileType);
        }

        [NonAction]
        public FileResult GetFileReadyToDownload(string rootPath, string fileName, string fileType)
        {
            IFileProvider provider = new PhysicalFileProvider(rootPath);
            IFileInfo fileInfo = provider.GetFileInfo(fileName);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = fileType;
            return File(readStream, mimeType, fileName);
        }

        [HttpGet("loggedin")]
        //This method should be called upon logging in. This checks if the user who is about to log in has registered if not, an account will be created for him.
        public async Task<IActionResult> CheckIfUserHasRegistered()
        {
            var user = await _userService.GetUserAsync(User.Claims
                .Single(e => e.Type.Equals(_nameIdentifierSchemaLocation)).Value);

            //If the user doesn't exist, add him to the database!
            if (user == null)
            {
                await _userService.AddUserAsync(new User()
                {
                    DisplayName = User.Claims
                      .Single(e => e.Type.Equals("name")).Value,
                    UserId = new Guid(User.Claims
                      .Single(e => e.Type.Equals(_nameIdentifierSchemaLocation)).Value)
                });
            }
            return RedirectToAction("Dashboard", "PrivateUser");
        }
    }
}