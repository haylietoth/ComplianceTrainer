using System.Web;
using System.Web.Optimization;

namespace ComplianceTrainer
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js",
                        "~/Scripts/angular.min.js",
                        "~/Scripts/angular-route.min.js",
                        "~/Scripts/angular-mocks.js",
                        "~/Scripts/site.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/dataTables/css").Include(
                      "~/Content/jquery.dataTables.min.css",
                      "~/Content/dataTables.bootstrap.min.css"                      
                      ));


            bundles.Add(new ScriptBundle("~/Scripts/dataTables/js").Include(
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/dataTables.bootstrap.min.js"
                      ));



        }
    }
}
