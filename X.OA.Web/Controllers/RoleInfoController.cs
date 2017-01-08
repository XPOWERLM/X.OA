using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.OA.IBLL;
using X.OA.Model;
using static X.OA.Common.Helper.UnityHelper;
using static X.OA.Common.Helper.JsonHelper;
using static X.OA.Common.Utility.StringUtility;
using Microsoft.Practices.Unity;

namespace X.OA.Web.Controllers
{
    public class RoleInfoController : BaseController
    {
        #region Create required objects
        IRoleInfoBLL rBLL = container.Resolve<IRoleInfoBLL>();
        #endregion
        // GET: RoleInfo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: RoleData
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="searchName"></param>
        /// <param name="searchRemark"></param>
        /// <returns></returns>
        public ActionResult RoleData(int page, int rows, string searchName, string searchRemark)
        {
            int totalCount = 0;
            var userData = !IsNullOrEmpty(true, searchName, searchRemark) ?
                rBLL.Retrieve(page, rows, out totalCount, u => true, u => u.Sort) :
                rBLL.Retrieve(page, rows, out totalCount, u => u.RoleName.Contains(searchName) || u.Remark.Contains(searchRemark), u => u.Sort);

            return JsonNT(new { total = totalCount, rows = userData });
        }

        [HttpPost]
        public ActionResult AddRole(RoleInfo entity)
        {
            // Validate
            if (!ModelState.IsValid) return JsonNT(new { Result = false, Msg = "Sth wrong" });

            entity.SubTime = DateTime.Now;
            entity.ModifiedOn = DateTime.Now.ToLongDateString();
            rBLL.Create(entity);
            bool result = rBLL.SaveChanges() > 0;
            return JsonNT(new { result = result, msg = result ? "Add success" : "Sth wrong" });
        }

        [HttpPost]
        public ActionResult DeleteRole(string userIDs)
        {
            if (string.IsNullOrEmpty(userIDs)) return JsonNT(new { result = false, msg = "parameter can't be null" });

            string[] idSplit = userIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<int> idArray = idSplit.Select(id => id.ToInt32());
            rBLL.Delete(idArray);
            bool result = rBLL.SaveChanges() > 0;
            return JsonNT(new { result = result, msg = result ? "Delete Success" : "Delete Failed" });
        }

        [HttpPost]
        public ActionResult EditRole(RoleInfo entity)
        {
            // Retrieve
            RoleInfo role = rBLL.Retrieve(u => u.ID == entity.ID).FirstOrDefault();

            // Update entity
            role.ModifiedOn = entity.ModifiedOn;
            role.Remark = entity.Remark;
            role.RoleName = entity.RoleName;

            // Update database
            rBLL.Update(role);
            bool result = rBLL.SaveChanges() > 0;

            // Reponse
            return JsonNT(new { result = result, msg = result ? "Update success." : "Update failed" });
        }

    }
}