namespace App
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery-3.1.1.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Content/External/DataTables/datatables.min.js",
                "~/Scripts/External/facebook.js",
                "~/Content/External/lightbox/js/lightbox.js",
                "~/Scripts/Internal/site.js",
                "~/Scripts/Internal/utils.js",
                "~/Scripts/Internal/index.js",
                "~/Scripts/Internal/infinite-scroller.js",
                "~/Scripts/Internal/datatables-manager.js",
                "~/Scripts/Internal/dashboard.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/beach-edit").Include(
                    "~/Scripts/jquery-ui.js",
                    "~/Scripts/External/html2canvas.min.js",
                    "~/Scripts/Internal/google-map-manager.js",
                    "~/Scripts/Internal/beaches.js",
                    "~/Scripts/Internal/blog.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/beach-add").Include(
                    "~/Scripts/jquery-ui.js",
                    "~/Scripts/Internal/google-map-manager.js",
                    "~/Scripts/Internal/beaches.js",
                    "~/Scripts/Internal/blog.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/beach-details").Include(
                    "~/Content/External/slick/slick.min.js",
                    "~/Content/External/lightGallery/js/lightgallery-all.min.js",
                    "~/Scripts/External/html2canvas.min.js",
                    "~/Scripts/Internal/infinite-scroller.js",
                    "~/Scripts/Internal/beach-details.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/review-details").Include(
                    "~/Content/External/slick/slick.min.js",
                    "~/Content/External/lightGallery/js/lightgallery-all.min.js",
                    "~/Scripts/External/rainbowvis.js",
                    "~/Scripts/External/dragdealer.min.js",
                    "~/Scripts/External/html2canvas.min.js",
                    "~/Scripts/Internal/dragdealers-manager.js",
                    "~/Scripts/Internal/review-details.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/review-post").Include(
                   "~/Scripts/External/dragdealer.min.js",
                   "~/Scripts/External/rainbowvis.js",
                   "~/Content/External/slick/slick.min.js",
                   "~/Content/External/lightGallery/js/lightgallery-all.min.js",
                   "~/Scripts/External/html2canvas.min.js",
                   "~/Scripts/Internal/dragdealers-manager.js",
                   "~/Scripts/Internal/reviews.js",
                   "~/Scripts/Internal/blog.js"
               ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
