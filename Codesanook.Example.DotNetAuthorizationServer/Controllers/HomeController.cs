using Microsoft.AspNetCore.Mvc;

namespace Codesanook.Example.DotNetAuthorizationServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
