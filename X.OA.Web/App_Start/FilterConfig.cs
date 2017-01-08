using System.Web;
using System.Web.Mvc;
using X.OA.Web.App_Start;

namespace X.OA.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
#if DEBUG 
            //filters.Add(new MyExceptionLogging());
#endif
        }
    }
}
