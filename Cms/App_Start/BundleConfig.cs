using DataLayer.Base;
using System.Web;
using System.Web.Optimization;

namespace Cms
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                "~/assets/KhoshKala/js/jquery.js"
                , "~/assets/KhoshKala/Js/popper.min.js"
                , "~/assets/KhoshKala/js/bootstrap.bundle.min.js"
                 , "~/assets/KhoshKala/Js/bootstrap-hover-dropdown.min.js"
                , "~/assets/plugins/knockout/knockout-3.4.2.js"
                , "~/assets/default/js/codeprocess.language.js"
                , "~/assets/plugins/toastr/js/toastr.min.js"
                , "~/assets/default/js/codeprocess.js"
               
                , "~/assets/KhoshKala/Js/owl.carousel.min.js"
                , "~/assets/KhoshKala/Js/ion.rangeSlider.min.js"
                , "~/assets/KhoshKala/Js/magiczoomplus.js"

                //, "~/assets/KhoshKala/Js/scripts.js"
                //, "~/assets/default/js/codeprocess.ajax.js"
                //, "~/assets/khoshkala/js/khoshkala_javascript19.js"
                ));
           
            bundles.Add(new ScriptBundle("~/bundles/script2").Include(
                "~/assets/BellaBrush/js/jquery.js"
                //, "~/assets/BellaBrush/js/bootstrap.bundle.min.js"
                , "~/assets/BellaBrush/js/owl.carousel.min.js"
                 , "~/assets/BellaBrush/js/magiczoomplus.js"
                , "~/assets/plugins/knockout/knockout-3.4.2.js"
                , "~/assets/default/js/codeprocess.language.js"
                , "~/assets/plugins/toastr/js/toastr.min.js"
                , "~/assets/default/js/codeprocess.js"
                //, "~/assets/BellaBrush/js/fontawesom.js"
                , "~/assets/BellaBrush/js/ion.rangeSlider.min.js"
                //, "~/assets/KhoshKala/Js/scripts.js"
                //, "~/assets/default/js/codeprocess.ajax.js"
                //, "~/assets/khoshkala/js/khoshkala_javascript19.js"
                ));
            bundles.Add(new StyleBundle("~/bundles/style").Include(
                "~/assets/KhoshKala/Css/animate.css"
                , "~/assets/KhoshKala/Css/owl.carousel.min.css"
                , "~/assets/KhoshKala/Css/owl.theme.default.min.css"
                , "~/assets/KhoshKala/Css/hover.css"
                , "~/assets/KhoshKala/Css/ion.rangeSlider.min.css"
                , "~/assets/KhoshKala/Css/magiczoomplus.css"
                , "~/assets/plugins/toastr/css/toastr.min.css"
                , "~/assets/KhoshKala/Css/style.css"
                //, "~/assets/Plugins/fileuploads/css/dropify.min.css"
                ));
            bundles.Add(new StyleBundle("~/bundles/style2").Include(
               "~/assets/BellaBrush/Css/owl.carousel.min.css"
               , "~/assets/BellaBrush/Css/owl.theme.default.min.css"
               , "~/assets/BellaBrush/Css/magiczoomplus.css"
               , "~/assets/BellaBrush/Css/font-awesome.min.css"
               , "~/assets/BellaBrush/Css/animate.css"
               , "~/assets/BellaBrush/Css/ion.rangeSlider.min.css"
               , "~/assets/BellaBrush/Css/hover.css"
               , "~/assets/plugins/toastr/css/toastr.min.css"
               , "~/assets/BellaBrush/Css/style.css"
               //, "~/assets/Plugins/fileuploads/css/dropify.min.css"
               ));
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}