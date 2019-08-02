using System.Web;
using System.Web.Optimization;

namespace Codesanook.Examples.AspNetMvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/script").Include("~/Scripts/main-bundle.js"));
            bundles.Add(new StyleBundle("~/style").Include( "~/Content/style.css"));
        }
    }
}
