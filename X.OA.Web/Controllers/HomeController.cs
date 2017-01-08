using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.OA.Model;
using static X.OA.Common.Helper.UnityHelper;
using Microsoft.Practices.Unity;
using X.OA.IBLL;

namespace X.OA.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewData.Model = userInfo;
            return View();
        }


        public ActionResult Test()
        {
            IUserInfoBLL uBLL = container.Resolve<IUserInfoBLL>();
            var content = uBLL.Retrieve(u => u.ID == 1).FirstOrDefault();
            return Content(content.ID.ToString());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}