using Enyim.Caching.Memcached;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using X.OA.IBLL;
using X.OA.Model;
using static X.OA.Common.Helper.JsonHelper;
using static X.OA.Common.Helper.MemcacheHelper;
using static X.OA.Common.Helper.UnityHelper;
using static X.OA.Common.Utility.LazyLoadUtility;
using static X.OA.Common.Utility.StringUtility;

namespace X.OA.Web.Controllers
{
    public class ActionInfoController : BaseController
    {
        #region Create required objects

        IActionInfoBLL aBLL = container.Resolve<IActionInfoBLL>();
        IRoleInfoBLL rBLL = container.Resolve<IRoleInfoBLL>();

        #endregion

        // GET: ActionInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RetrieveAction(int page, int rows, string searchName, string searchRemark)
        {
            int totalCount = 0;
            var userData = !IsNullOrEmpty(true, searchName, searchRemark) ?
            aBLL.Retrieve(page, rows, out totalCount, u => true, u => u.ID) :
            aBLL.Retrieve(page, rows, out totalCount, u => u.ActionInfoName.Contains(searchName) && u.Remark.Contains(searchRemark), u => u.Sort);

            return JsonNT(new { total = totalCount, rows = userData });

        }

        [HttpPost]
        public ActionResult CreateAction(ActionInfo entity)
        {
            // Validate
            if (!ModelState.IsValid || entity == null)
                return JsonNT(new { result = false, msg = "Sth wrong" });

            entity.ModifiedOn = DateTime.Now.ToString();
            entity.SubTime = DateTime.Now;
            entity.DelFlag = 0;

            // Create
            aBLL.Create(entity);
            bool result = aBLL.SaveChanges() > 0;

            // Response
            return JsonNT(new { result = result, msg = result ? "Add success" : "Add failed" });
        }

        public ActionResult Edit(int id)
        {
            return View(aBLL.Retrieve(a => a.ID == id).First());
        }

        [HttpPost]
        public ActionResult Edit(ActionInfo model)
        {
            if (!ModelState.IsValid)
                return JsonNT(new { result = false, msg = "Sth wrong" });

            //ActionInfo action = aBLL.Retrieve(a => a.ID == model.ID).First();
            aBLL.Update(model);
            bool result = aBLL.SaveChanges() > 0;

            return JsonNT(new { result = result, msg = result ? "Edit success" : "Edit failed" });
        }

        public ActionResult DeleteAction()
        {
            return View();
        }

        public ActionResult SetRole(int id)
        {
            ActionInfo action = aBLL.Retrieve(a => a.ID == id).First();
            IEnumerable<RoleInfo> roles = rBLL.Retrieve(r => true);
            ViewBag.action = action;
            ViewBag.roles = roles;
            return View();
        }

        [HttpPost]
        public ActionResult SetRole(int actionId, string roleIds)
        {
            IEnumerable<int> roleIdArray = roleIds.Split(',').Select(id => id.ToInt32());

            if (!string.IsNullOrEmpty(roleIds) && roleIdArray.Count() > 0)
                aBLL.SetRole(actionId, roleIdArray);
            bool result = aBLL.SaveChanges() > 0;

            // Response
            return JsonNT(new { result = result, msg = result ? "Set role success" : "Set role failed" });
        }

        public ActionResult Upload()
        {
            if (Request.Files?.Count != 1)
                return JsonNT(new { result = false, msg = "File count illegal" });

            HttpPostedFileBase file = Request.Files[0];
            string extension = Path.GetExtension(file.FileName);
            string guidPath = Guid.NewGuid().ToString("N");

            // Important! check the file format
            if (!MatchWhiteList(extension))
                return JsonNT(new { result = false, msg = "File format illegal" });

            string virtualPath = Request.MapPath($"/Content/Images/MenuIcon/{guidPath}{extension}");
            file.SaveAs(virtualPath);

            return JsonNT(new { result = true, msg = $"{guidPath}{extension}" });
        }

        #region Auxiliary routines

        /// <summary>
        /// Check file format
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool MatchWhiteList(string extension)
        {
            return new Regex(".jpg|.bmp|.png").IsMatch(extension);
        }
        #endregion
    }
}