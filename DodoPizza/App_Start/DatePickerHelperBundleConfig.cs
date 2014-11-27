using System.Web.Optimization;

namespace DodoPizza.App_Start
{
	public class DatePickerHelperBundleConfig
	{
		public static void RegisterBundles()
		{
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
            "~/Scripts/bootstrap-datepicker.js",
            "~/Scripts/moment.js",
            "~/Scripts/moment-with-locales.js",
            "~/Scripts/bootstrap-datetimepicker.js",
            "~/Scripts/locales/bootstrap-datepicker.*"));

            BundleTable.Bundles.Add(new StyleBundle("~/Content/datepicker").Include(
            "~/Content/bootstrap-datepicker.css",
            "~/Content/bootstrap-datetimepicker.css"));
		}
	}
}