using Microsoft.Practices.Unity;
using System;
using System.Web.Mvc;
using X.OA.Common.Helper;
using X.OA.IBLL;
using X.OA.Model;
using static Newtonsoft.Json.JsonConvert;
using static X.OA.Common.Helper.UnityHelper;
using static X.OA.Common.Utility.PasswordUtility;
using static X.OA.Common.Utility.StringUtility;

namespace X.OA.Web.Controllers
{
    public class SignUpController : Controller, System.Web.SessionState.IRequiresSessionState
    {
        #region Create required objects

        IUserInfoBLL uBLL = container.Resolve<IUserInfoBLL>();

        #endregion
        // GET: SignUp
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: SignUp
        /// </summary>
        /// <param name="UName"></param>
        /// <param name="UPwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignUp(string UName, string UPwd)
        {
            // Validate
            if (!CAPTCHAHelper.CheckCAPTCHA)
                return Json(new { result = false, msg = "CAPTCHA Wrong!" });
            if (IsNullOrEmpty(false, UName, UPwd))
                return Json(new { result = false, msg = "Sth Wrong!" });

            // Generate PBKDF2 hash
            string PBKDF2 = CreateHash(UPwd);
            uBLL.Create(new UserInfo { UName = UName, UPwd = PBKDF2, SubTime = DateTime.Now, ModifiedOn = DateTime.Now });
            bool result = uBLL.SaveChanges() > 0;

            // Response
            if (result)
                return Content(SerializeObject(new { result = result, msg = "SignUp success!" }), "application/json");
            else
                return Content(SerializeObject(new { result = result, msg = "SignUp failed!" }), "application/json");

        }
    }
}