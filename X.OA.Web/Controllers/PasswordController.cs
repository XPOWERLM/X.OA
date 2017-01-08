using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using X.OA.Common.Utility;
using X.OA.IBLL;
using X.OA.Model;
using X.OA.Web.Properties;
using static X.OA.Common.Helper.MemcacheHelper;
using static X.OA.Common.Helper.UnityHelper;
using static X.OA.Common.Utility.LazyLoadUtility;
using static X.OA.Common.Utility.StringUtility;
using static X.OA.Common.Utility.PasswordUtility;
using Enyim.Caching.Memcached;

namespace X.OA.Web.Controllers
{
    public class PasswordController : Controller
    {
        #region Create required objects
        IUserInfoBLL uBLL = container.Resolve<IUserInfoBLL>();
        #endregion
        // GET: Password
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reset(string iSigninName, string iSigninEmail)
        {
            if (IsNullOrEmpty(false, iSigninName, iSigninEmail))
                return JsonNT(new { Result = false, Msg = "Sth wrong" });

            var retrieveResult = uBLL.Retrieve(u => u.UName == iSigninName && u.email == iSigninEmail).FirstOrDefault();

            if (retrieveResult != null)
            {
                string resetCode = Guid.NewGuid().ToString("N").Substring(0, 7);
                string mailContent = Resources.ResetEmail.Replace("$email", retrieveResult.UName).Replace("$resetCode", resetCode);
                // Store reset id
                memcachedClient.Store(Enyim.Caching.Memcached.StoreMode.Set, resetCode, LazyLoadConvert<UserInfo>(retrieveResult));
                // Send email
                MailUtility.SendMail(new MailMessage { To = retrieveResult.email, BodyFormat = MailFormat.Html, Subject = "密码重置", Body = mailContent });
                // Set cookie
                Response.Cookies.Add(new HttpCookie("resetEmail") { Value = retrieveResult.email });
                // Response
                return JsonNT(new { Result = true, Msg = "The email already sent to your mailbox" });
            }

            return JsonNT(new { Result = false, Msg = "Sth wrong" });
        }

        /// <summary>
        /// GET: Verify
        /// </summary>
        /// <returns></returns>
        public ActionResult Verify()
        {
            // Check cookie
            string email = Request.Cookies["resetEmail"]?.Value;
            // Clear cookie
            Response.Cookies.Add(new HttpCookie("resetEmail") { Value = string.Empty, Expires = DateTime.Now.AddDays(-10) });

            if (string.IsNullOrEmpty(email)) return Redirect("/Password");
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public ActionResult Verify(string resetCode, string resetPassword)
        {
            if (resetCode == null) return JsonNT(new { Result = false });

            // Check reset code
            UserInfo user = memcachedClient.Get<UserInfo>(resetCode.Trim());
            // Clear reset code
            memcachedClient.Store(StoreMode.Set, resetCode, null);
            if (user != null)
            {
                // Change password
                user = uBLL.Retrieve(u => u.ID == user.ID).FirstOrDefault();
                user.UPwd = CreateHash(resetPassword);
                uBLL.Update(user);
                if (uBLL.SaveChanges() > 0)
                    return JsonNT(new { Result = true, Msg = "Your password success changed" });
            }
            return JsonNT(new { Result = false, Msg = "Sth wrong" });
        }


        #region Auxiliary routines
        public ActionResult JsonNT(object obj)
        {
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }

        #endregion
    }
}