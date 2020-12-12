using System.Web;
using System.Web.Mvc;

namespace INFO_TRACK_WEB_SCRAPER
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
