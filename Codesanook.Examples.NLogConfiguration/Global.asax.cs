using NLog;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Codesanook.Examples.NLogConfiguration
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            LogManager.ThrowExceptions = true;
            //LogManager.Configuration = LogManager.Configuration.Reload();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
