using Enyim.Caching.Memcached;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.OA.Common.Helper;
using X.OA.IBLL;
using X.OA.Model;
using X.OA.Web.Properties;
using static Newtonsoft.Json.JsonConvert;
using static X.OA.Common.Helper.MemcacheHelper;
using static X.OA.Common.Helper.UnityHelper;
using static X.OA.Common.Utility.PasswordUtility;
using static X.OA.Common.Utility.LazyLoadUtility;

namespace X.OA.Web.Controllers
{
    public class SignInController : Controller, System.Web.SessionState.IRequiresSessionState
    {
        #region Create required objects

        IUserInfoBLL uBll = container.Resolve<IUserInfoBLL>();

        #endregion

        /// <summary>
        /// GET: Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Auto login
            string sessionId = Request.Cookies[Resources.SessionIdName]?.Value;
            if (!string.IsNullOrEmpty(sessionId))
            {
                var sessionValue = memcachedClient.Get<UserInfo>(sessionId);
                if (sessionValue != null)
                    return Redirect("/Home");
            }


            return View();
        }

        /// <summary>
        /// GET: SignIn
        /// </summary>
        /// <returns></returns>
        public ActionResult SignIn(UserInfo entity)
        {
            // Validate
            if (!CAPTCHAHelper.CheckCAPTCHA)
                return Json(new { result = false, msg = "CAPTCHA Wrong!" });
            if (!ModelState.IsValid)
                return Json(new { result = false, msg = "Sth wrong" });

            // Retrieve
            UserInfo retrieveResult = uBll.Retrieve(u => u.UName == entity.UName).FirstOrDefault() as UserInfo;

            // Important, if you enabled lazy load, this procedure is necessary
            retrieveResult = LazyLoadConvert<UserInfo>(retrieveResult);

            // Response
            if (retrieveResult != null)
            {
                // Validate password
                if (ValidatePassword(entity.UPwd, retrieveResult.UPwd))
                {
                    // Set session
                    string memcacheSessionId = Guid.NewGuid().ToString("N");
                    bool storeResult = memcachedClient.Store(StoreMode.Set, memcacheSessionId, retrieveResult);

                    // Remember user
                    if (Request["rememberMe"] == "on")
                        Response.Cookies.Set(new HttpCookie(Resources.SessionIdName) { Value = memcacheSessionId, Expires = DateTime.Now.AddDays(15) });
                    else
                        Response.Cookies.Set(new HttpCookie(Resources.SessionIdName) { Value = memcacheSessionId });

                    // Sign in Success
                    return Json(new { result = true, msg = "Sign in Success" });
                }
            }
            // Sign in Failed
            return Json(new { result = false, msg = "Sign in Failed" });
        }

        /// <summary>
        /// GET: ResetPassword
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(UserInfo entity)
        {
            var retrieveResult = uBll.Retrieve(u => u.UName == entity.UName && u.Remark == entity.Remark).FirstOrDefault();
            if (retrieveResult != null)
            {
                (uBll as IUserInfoBLL).ResetPassword(entity);
                return Content(SerializeObject(new { msg = "A email contains new password already sent to your eamil", result = true }));
            }
            else
                return Content(SerializeObject(new { msg = "User and mail doesn't match", result = false }), "application/json");
        }
    }
}