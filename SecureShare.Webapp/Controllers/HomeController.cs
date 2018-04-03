using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecureShare.Website.ViewModels;

namespace SecureShare.Webapp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult Pricing()
		{
			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
		}
	}
}