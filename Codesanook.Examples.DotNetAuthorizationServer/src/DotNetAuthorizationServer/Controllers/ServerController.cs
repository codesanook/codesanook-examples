using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace AuthorizeServer.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {        public IActionResult Index()
        {
            return Ok("Hello world");
        }
    }
}

