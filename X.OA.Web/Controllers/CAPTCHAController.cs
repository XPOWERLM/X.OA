using System.Web.Mvc;
using static X.OA.Common.Helper.CAPTCHAHelper;

namespace X.OA.Web.Controllers
{
    public class CAPTCHAController : Controller, System.Web.SessionState.IRequiresSessionState
    {
        // GET: CAPTCHA
        public ActionResult Index()
        {
            // Generate ramdom string
            string verifyCode = CreateVerifyCode();
            // Set session
            Session["CAPTCHA"] = verifyCode;
            // Rsponse
            return File(CreateBytesCode(verifyCode), "image/jpeg");
        }
    }
}