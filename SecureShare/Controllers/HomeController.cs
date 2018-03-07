using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureShare.Models;

namespace SecureShare.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
//            var client = new HttpClient();
//            var message = new HttpRequestMessage(HttpMethod.Get, "http://localhost:55555/api/values");
//            var response = await client.SendAsync(message);
//            var text = await response.Content.ReadAsStringAsync();
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
