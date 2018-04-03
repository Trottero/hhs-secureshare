using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SecureShare.Webapp.Models;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services;
using SecureShare.WebApi.Wrapper.Services.Interfaces;

namespace SecureShare.Webapp.Controllers
{
	[Route("dashboard")]
	[Authorize]
	public class PrivateUserController : Controller
	{
		private readonly IUserFileService _userFileService;
		private readonly IUserService _userService;
		private readonly IShareFileService _shareFileService;

		private const string NameIdentifierSchemaLocation =
			"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

		public PrivateUserController(IUserFileService userFileService, IUserService userService,
			IShareFileService shareFileService)
		{
			_userFileService = userFileService;
			_userService = userService;
			_shareFileService = shareFileService;
		}

		[HttpGet("myfiles")]
		public async Task<IActionResult> MyFiles()
		{
			var userId = User.Claims
				.Single(e => e.Type.Equals(NameIdentifierSchemaLocation)).Value;
			var files = await _userFileService.GetFilesFromUserAsync(userId);
			return View(files);
		}

		[HttpGet("sharedwithme")]
		public async Task<IActionResult> SharedWithUser()
		{
			var userId = User.Claims
				.Single(e => e.Type.Equals(NameIdentifierSchemaLocation)).Value;
			var files = await _shareFileService.GetSharedWithUserAsync(userId);
		    if (files == null)
		    {
                files = new List<UserFile>();
		    }
			return View(files);
		}


		[HttpGet("upload")]
		public IActionResult Upload()
		{
			return View();
		}


		[HttpPost("upload")]
		public async Task<IActionResult> Upload(IFormFile file, string sharedWith)
		{
			if (Guid.TryParse(sharedWith, out _))
			{
				var ownerId = new Guid(User.Claims
					.Single(e => e.Type.Equals(NameIdentifierSchemaLocation)).Value);
				var sharedUser = await _userService.GetUserAsync(sharedWith);
				await _shareFileService.AddSharedFile(file, ownerId, sharedUser);
				return RedirectToAction("MyFiles");
			}
			await _userFileService.AddUserFileAsync(file,
				new Guid(User.Claims.Single(e => e.Type.Equals(NameIdentifierSchemaLocation)).Value));
			return RedirectToAction("MyFiles");
		}

		[HttpGet("download")]
		public async Task<IActionResult> DownloadFile(Guid id)
		{
			var userFile = await _userFileService.GetUserFileAsync(id);
			var downloadInfo = await _userFileService.GetUserFileDownloadPathAsync(userFile);
			return GetFileReadyToDownload(downloadInfo.RootPath, downloadInfo.FileName, downloadInfo.FileType);
		}

		[HttpGet("loggedin")]
		//This method should be called upon logging in. This checks if the user who is about to log in has registered if not, an account will be created for him.
		public async Task<IActionResult> CheckIfUserHasRegistered()
		{
			var user = await _userService.GetUserAsync(User.Claims
				.Single(e => e.Type.Equals(NameIdentifierSchemaLocation)).Value);

			//If the user doesn't exist, add him to the database!
			if (user == null)
			{
				await _userService.AddUserAsync(new User()
				{
					DisplayName = User.Claims
						.Single(e => e.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")).Value,
					UserId = new Guid(User.Claims
						.Single(e => e.Type.Equals(NameIdentifierSchemaLocation)).Value)
				});
			}

			return RedirectToAction("MyFiles", "PrivateUser");
		}

        [HttpGet("share/{id}")]
	    public async Task<IActionResult> ShareFileWithUser(Guid id)
        {
            var userFile = await _userFileService.GetUserFileAsync(id);
            if (userFile == null)
            {
                return NotFound();
            }
            var viewModel = new ShareFileViewModel
            {
                FileToShare = userFile.UserFileId,
                Filename = userFile.FileName
            };
            return View(viewModel);
        }

	    [HttpPost("share/{id}")]
        [ValidateAntiForgeryToken]
	    public async Task<IActionResult> ShareFileWithUser([Bind("FileToShare,UserToShareWith,Filename")] ShareFileViewModel shareFileViewModel, Guid id)
	    {
	        if (!ModelState.IsValid)
	        {
	            return NotFound();
	        }
	        var userToShareWith = await _userService.GetUserAsync(shareFileViewModel.UserToShareWith);
	        if (userToShareWith == null)
	        {
	            ViewData["usererror"] = "That user does not exist";
	            return View(shareFileViewModel);
	        }
	        await _shareFileService.ShareFileAsync(shareFileViewModel.FileToShare, shareFileViewModel.UserToShareWith);
	        return RedirectToAction("MyFiles");
	    }

	    [HttpGet("delete/{id}")]
	    public async Task<IActionResult> DeleteFile(Guid id)
	    {
	        await _userFileService.DeleteUserFileAsync(id);
	        return RedirectToAction("MyFiles");
	    }

		[NonAction]
		private FileResult GetFileReadyToDownload(string rootPath, string fileName, string fileType)
		{
			var provider = new PhysicalFileProvider(rootPath);
			var fileInfo = provider.GetFileInfo(fileName);
			var readStream = fileInfo.CreateReadStream();
			var mimeType = fileType;
			return File(readStream, mimeType, fileName);
		}
	}
}