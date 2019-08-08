using NLog;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Codesanook.Examples.NLogConfiguration.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public async Task<ActionResult> Index()
        {
            logger.Info($"first logger thread id {Thread.CurrentThread.ManagedThreadId}");
            await LongRunningProcess();
            logger.Info($"second logger thread id {Thread.CurrentThread.ManagedThreadId}");
            return View();
        }

        private Task LongRunningProcess()
        {
            var task = Task.Run(() =>
             {
                 Thread.Sleep(2000);
                 var localLogger = LogManager.GetCurrentClassLogger();
                 localLogger.Info($"local logger thread id {Thread.CurrentThread.ManagedThreadId}");
             });
            return task;
        }
    }
}