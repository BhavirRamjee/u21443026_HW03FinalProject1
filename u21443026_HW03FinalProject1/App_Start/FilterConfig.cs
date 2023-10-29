using System.Web;
using System.Web.Mvc;

namespace u21443026_HW03FinalProject1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
