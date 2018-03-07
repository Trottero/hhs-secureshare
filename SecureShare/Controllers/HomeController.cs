using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;


namespace SecureShare.Website.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IUserService _userService;

		public HomeController(IUserService userService)
		{
			_userService = userService;
		}

		public IActionResult Index()
		{
			var user = new User
			{
				DisplayName = "test",
				UserId = Guid.NewGuid()
			};

//			ViewData["Message"] = _userService.GetAllUsers();


			return View("Index");
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		[AllowAnonymous]
		public IActionResult Error()
		{
			return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
		}
	}
}