using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace SecureShare.Website.Controllers
{
	[Route("[controller]/[action]")]
	public class AccountController : Controller
	{
		[HttpGet]
		public IActionResult SignIn()
		{
			var redirectUrl = Url.Action("CheckIfUserHasRegistered", "PrivateUser");
			return Challenge(
				new AuthenticationProperties {RedirectUri = redirectUrl},
				OpenIdConnectDefaults.AuthenticationScheme);
		}

		[HttpGet]
		public IActionResult SignOut()
		{
			var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);
			return SignOut(
				new AuthenticationProperties {RedirectUri = callbackUrl},
				CookieAuthenticationDefaults.AuthenticationScheme,
				OpenIdConnectDefaults.AuthenticationScheme);
		}

		[HttpGet]
		public IActionResult SignedOut()
		{
			if (User.Identity.IsAuthenticated) return RedirectToAction("Dashboard", "PrivateUser");

			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}