using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureShare.WebApi.Wrapper.Models;
using SecureShare.WebApi.Wrapper.Services.Interfaces;
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

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Profile()
		{
			var oneUser = await _userService.GetUserAsync("c5122eb9-1fb4-4a5b-8e01-0a1d99fc6619");
			return View(oneUser);
		}

		[AllowAnonymous]
		public IActionResult Error()
		{
			return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
		}
	}
}