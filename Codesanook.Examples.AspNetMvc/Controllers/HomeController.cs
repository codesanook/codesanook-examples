using NLog;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Codesanook.Examples.AspNetMvc.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public async Task<ActionResult> Index()
        {
            LogThreadInformation("First");
            await LongRunningProcess();
            LogThreadInformation("Second");
            return View();
        }

        private void LogThreadInformation(string name)
        {
            var thread = Thread.CurrentThread;
            logger.Info(
                $"{name} thread id {thread.ManagedThreadId}, is thread pool {thread.IsThreadPoolThread}"
            );
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        private Task LongRunningProcess()
        {
            var task = Task.Run(() =>
             {
                 Thread.Sleep(2000);
                 LogThreadInformation("Local");
             });
            return task;
        }
    }
}
