using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
	}
}