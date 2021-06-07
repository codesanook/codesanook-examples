using Codesanook.Examples.DotNetAuthorizationServer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Codesanook.Examples.DotNetAuthorizationServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel() { InitialValue = 999 };
            return View(viewModel);
        }
    }
}
