using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace AuthorizeServer.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class CheckController : ControllerBase
    {

        private readonly ILogger<CheckController> _logger;

         public CheckController(ILogger<CheckController> logger)
        {
            _logger = logger;
        }
        [HttpGet("public")]
        public IActionResult Index()
        {
            _logger.LogInformation("Hello Anonymous");
            return Ok("Hello Anonymous");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("private")]
         public IActionResult Protect()
        {
            _logger.LogInformation("You're signed in");
            return Ok("You're signed in");
        }
    }
}

