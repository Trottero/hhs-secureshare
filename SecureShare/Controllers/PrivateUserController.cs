using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SecureShare.WebApi.Wrapper.Services.Interfaces;

namespace SecureShare.Website.Controllers
{
    [Route("user")]
    [Authorize]
    public class PrivateUserController : Controller
    {
        private readonly IUserFileService _userFileService;
        public PrivateUserController(IUserFileService userFileService)
        {
            _userFileService = userFileService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var id = new Guid(User.Claims
                .Single(e => e.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier")).Value);
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
            var userfile = await _userFileService.AddUserFileAsync(file, new Guid(User.Claims.Single(e => e.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier")).Value));
            return RedirectToAction("Dashboard");
        }

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
    }
}