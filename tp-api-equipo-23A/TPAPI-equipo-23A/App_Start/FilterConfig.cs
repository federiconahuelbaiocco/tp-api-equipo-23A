using System.Web;
using System.Web.Mvc;

namespace TPAPI_equipo_23A
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
