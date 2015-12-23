using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                      "~/Scripts/js.net.md5.js",
                      "~/Scripts/linq.js",
                      "~/Scripts/linq.array.js",
                      "~/Scripts/linq.jquery.js",
                      "~/Scripts/leaflet-0.7.3.js",
                      "~/Scripts/highlight.pack.js",
                      "~/Scripts/app/appEvents.js",
                      "~/Scripts/app/app.signalR.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/leaflet.css",
                      "~/Content/site.css",
                      "~/Content/hl_styles/tomorrow-night-eighties.css"));
        }
    }
}
