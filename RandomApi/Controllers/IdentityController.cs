using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RandomApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class IdentityController : ControllerBase
	{

		private readonly ILogger<IdentityController> _logger;

		public IdentityController(ILogger<IdentityController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Index()
		{
			_logger.LogInformation($"user claims => {User.Claims}");
			return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
		}
	}
}