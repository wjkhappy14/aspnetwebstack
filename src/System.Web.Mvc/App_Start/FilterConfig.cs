using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.MyFilters;

namespace System.Web.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RemoveDuplicateContentAttribute());
        }
    }
}
