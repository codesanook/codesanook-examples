using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;

namespace AuthorizeServer.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {

        private readonly ILogger<ServerController> _logger;

         public ServerController(ILogger<ServerController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("Test Logger");
            return Ok("Hello world");
        }
    }
}

