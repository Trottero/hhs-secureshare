using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;
using SecureShare.Website.Extensions;
using SecureShare.Website.ViewModels;


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

		public async Task<IActionResult> Index()
		{
			var user = new User
			{
				DisplayName = "test",
				UserId = Guid.NewGuid()
			};

			ViewData["Message1"] = await _userService.AddUser(user);
			ViewData["Message2"] = await _userService.GetAllUsers();


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