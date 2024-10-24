using EventsMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EventsMVC.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private string baseurl = "https://localhost:7191/api/EventMasters/";
        HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "UserMasters");
			}

			ViewBag.UserEmail = HttpContext.Session.GetString("UserName");
			ViewBag.UserID = HttpContext.Session.GetString("UserId");

            var reponse = await client.GetAsync(baseurl);
            var reposenValue = await reponse.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventMaster>>(reposenValue);
            return View(events);
        }

		public IActionResult Privacy()
		{
			return View();
		}


      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
