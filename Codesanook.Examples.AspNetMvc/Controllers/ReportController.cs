using Codesanook.Examples.AspNetMvc.Reports;
using System.Web.Mvc;

namespace Codesanook.Examples.AspNetMvc.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            var sqlconn_test = new SqlConnectionJtds();
            sqlconn_test.ExpReort(JtdsConnection.TASK_PDF);
            return Content("Exported");
        }
    }
}
