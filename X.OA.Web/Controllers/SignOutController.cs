using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.OA.Web.Properties;
using static Newtonsoft.Json.JsonConvert;
using static X.OA.Common.Helper.MemcacheHelper;

namespace X.OA.Web.Controllers
{
    public class SignOutController : Controller
    {
        // GET: SignOut
        public ActionResult Index()
        {
            string sessionId = Request.Cookies[Resources.SessionIdName]?.Value;
            // Clear session
            memcachedClient.Store(StoreMode.Set, sessionId, null);
            // Clear cookie
            Response.Cookies.Set(new HttpCookie(sessionId) { Expires = DateTime.Now.AddDays(-1), Value = null });
            return View();
        }
    }
}