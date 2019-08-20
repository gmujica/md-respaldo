using MesaDinero.Admin.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MvcAuthorizeAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
