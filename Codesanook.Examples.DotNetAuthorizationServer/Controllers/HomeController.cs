using Microsoft.AspNetCore.Mvc;

namespace Codesanook.Examples.DotNetAuthorizationServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
