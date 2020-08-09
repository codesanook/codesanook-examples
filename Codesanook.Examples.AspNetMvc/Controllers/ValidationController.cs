using Codesanook.Examples.AspNetMvc.Models;
using System.Web.Mvc;

namespace Codesanook.Examples.AspNetMvc.Controllers
{
    public class ValidationController : Controller
    {
        // GET: Validation
        public ActionResult Index() => View();
        public ActionResult Create() => View();

        [HttpPost]
        public ActionResult Create(User user) => View();
    }
}
