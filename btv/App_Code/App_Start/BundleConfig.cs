using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/js/chosen.jquery.min.js",
                            "~/js/uniform.jquery.js",
                            "~/js/sticky.full.js",
                            "~/js/jquery.noty.js",
                            "~/js/notify.min.js",
                            "~/js/selectToUISlider.jQuery.js",
                            "~/js/bootstrap-colorpicker.js",
                            "~/js/bootstrap-dropdown.js",
                            "~/js/jquery.tipsy.js",
                            "~/js/accordion.jquery.js",
                            "~/js/autogrow.jquery.js",
                            "~/js/inputmask.jquery.js",
                            "~/js/stepy.jquery.js",
                            "~/js/vaidation.jquery.js",
                            "~/js/custom-scripts.js"));


            /* bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                             "~/js/jquery-ui-1.9.2.custom.min.js"));
             bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                             "~/js/jquery-1.8.3.min.js"));
             bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                             "~/js/bootstrap.min.js"));

             bundles.Add(new ScriptBundle("~/bundles/TopScripts").Include(
                             "~/js/iphone-style-checkboxes.js",
                             "~/js/select2.min.js"));
                             */
            // Code removed for clarity.
            BundleTable.EnableOptimizations = true;
        }
    }
}