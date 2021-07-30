using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAuthorizationServer.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [Route("api/users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUsers(int userId)
        {
            return Json(new[] {
                new { FirstName = "Jose", LastName = "Realman" }
            });
        }
    }
}
