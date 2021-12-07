using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace dbShopeeAutomationV2.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.6.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsPDF").Include("~/Scripts/jspdf.min.js", "~/Scripts/html2PDF.js"));

            bundles.Add(new ScriptBundle("~/bundles/QRCode").Include("~/Scripts/qrcode.min.js", "~/Scripts/html5-qrcode.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.min.css", "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css/fontawesome").Include("~/Content/fontawesome.min.css", "~/Content/importFontAwesomeFont.css"));
        }
    }
}