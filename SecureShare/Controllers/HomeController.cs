using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;
using SecureShare.Website.ViewModels;


namespace SecureShare.Website.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IUserService _userService;
		private readonly IUserFileService _userFileService;

		public HomeController(IUserService userService, IUserFileService userFileService)
		{
			_userService = userService;
			_userFileService = userFileService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Profile()
		{
			await _userFileService.GetUserFileAsync(new Guid("62769c76-f62f-46fa-52bf-08d58a9bcddb"));
			
			var oneUser = await _userService.GetUserAsync("181af0d2-3667-4402-a3c6-14f05a2a76ed");
			return View(oneUser);
		}

		[AllowAnonymous]
		public IActionResult Error()
		{
			return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
		}

	    public async Task<IActionResult> DisplayMyClaims()
	    {
	        var claims = User.Claims;
	        return View(claims);
	    }

	    public async Task<IActionResult> AddMeToDatabase()
	    {
	        await _userService.AddUserAsync(new User()
	        {
                DisplayName =  User.Claims.Single(e => e.Type.Equals("name")).Value,              
                UserId = new Guid(User.Claims.Single(e => e.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier")).Value)
            });
	        return Ok();
	    }

	    public async Task<IActionResult> UploadAFile()
	    {
	        return View();
	    }

        [HttpPost]
	    public async Task<IActionResult> Upload(IFormFile file)
        {
            var userfile = await _userFileService.AddUserFileAsync(file, new Guid(User.Claims.Single(e => e.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier")).Value));
            return View(userfile);
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