﻿using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RandomMVC.Models;

namespace RandomMVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
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

		public IActionResult Logout()
		{
			return SignOut("Cookies", "oidc");
		}

		public IActionResult Login()
		{
			return Challenge(new AuthenticationProperties
			{
				RedirectUri = "/Home/Index"
			}, "oidc");
		}
		public async Task<IActionResult> CallApi()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");

			var client = new HttpClient();
			client.SetBearerToken(accessToken);
			// client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			var content = await client.GetStringAsync("http://localhost:5001/identity");

			ViewBag.Json = JArray.Parse(content).ToString();
			return View("json");
		}
	}
}