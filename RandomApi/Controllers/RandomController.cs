using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RandomApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RandomController : ControllerBase
	{

		private readonly ILogger<RandomController> _logger;
		private readonly Random random = new Random();
		public RandomController(ILogger<RandomController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var randomInt = random.Next();
			_logger.LogInformation($"RandomInt => {randomInt}");
			return new JsonResult(new
			{
				Number = randomInt
			});
		}
	}
}