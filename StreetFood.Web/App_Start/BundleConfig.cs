using System.Web.Optimization;

namespace StreetFood.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var siteCss = new StyleBundle("~/Content/css")
                .Include("~/Content/*.css");

            siteCss.Transforms.Add(new CssMinify());
            bundles.Add(siteCss);

            var siteJs = new ScriptBundle("~/Scripts/js")
                .Include("~/Scripts/*.js");
            siteJs.Transforms.Add(new JsMinify());
            bundles.Add(siteJs);
        }
    }
}
