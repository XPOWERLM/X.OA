using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.OA.IBLL;
using Microsoft.Practices.Unity;
using static X.OA.Common.Helper.UnityHelper;
using static X.OA.Common.Helper.JsonHelper;
using X.OA.Model;

namespace X.OA.Web.Controllers
{
    public class WFTemplateController : BaseController
    {
        #region Create required objects
        IWF_TempBLL wfTempBLL = container.Resolve<IWF_TempBLL>();

        #endregion

        // GET: WFTemplate
        public ActionResult Index()
        {
            return View(wfTempBLL.Retrieve(w => true));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(WF_Temp model)
        {
            model.SubBy = userInfo.ID;
            model.SubTime = DateTime.Now;
            model.ModfiedOn = DateTime.Now;
            model.DelFlag = 0;
            model.TempStatus = 0;
            wfTempBLL.Create(model);
            bool result = wfTempBLL.SaveChanges() > 0;
            return JsonNT(new { result = result, msg = result ? "Create template success" : "Create template failed" });
        }

        public ActionResult Delete(int id)
        {
            WF_Temp template = wfTempBLL.Retrieve(t => t.ID == id).FirstOrDefault();
            wfTempBLL.Delete(template);
            bool result = wfTempBLL.SaveChanges() > 0;
            return JsonNT(new { result = result, msg = result ? "Delete template success" : "Delete template failed" });
        }
    }
}