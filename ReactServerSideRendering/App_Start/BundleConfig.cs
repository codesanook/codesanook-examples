using React;
using System.Web;
using System.Web.Optimization;

namespace ReactServerSideRendering
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            ReactSiteConfiguration.Configuration.AddScript("~/scripts/server.bundle.js"); 
        }
    }
}
