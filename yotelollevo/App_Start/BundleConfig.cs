using System.Web;
using System.Web.Optimization;

namespace yotelollevo
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundle/jquery").Include(
    "~/Scripts/jquery-3.7.1.min.js"
));

            bundles.Add(new ScriptBundle("~/bundle/bootstrap").Include(
                "~/Scripts/bootstrap.bundle.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/datatables").Include(
                "~/Scripts/datatables/jquery.dataTables.min.js",
                "~/Scripts/datatables/dataTables.bootstrap5.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/site").Include(
                "~/Scripts/site.js"
            ));


            // CSS Bootstrap 5
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.min.css"
            ));

            // CSS DataTables Bootstrap 5
            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                "~/Content/datatables/dataTables.bootstrap5.min.css"
            ));

            // CSS propio
            bundles.Add(new StyleBundle("~/Content/site").Include(
                "~/Content/site.css"
            ));

            BundleTable.EnableOptimizations = false; // en desarrollo
        }
    }
}
