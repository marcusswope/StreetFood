using System.Web.Mvc;
using System.Web.Routing;

namespace StreetFood.Web
{
    public class FilterConfig
    {
        public static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthenticationFilter());
        }
    }
}
