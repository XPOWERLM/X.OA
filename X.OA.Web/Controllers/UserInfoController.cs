using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using X.OA.IBLL;
using X.OA.Model;
using static X.OA.Common.Helper.UnityHelper;
using static X.OA.Common.Utility.StringUtility;
using static X.OA.Common.Helper.JsonHelper;

namespace X.OA.Web.Controllers
{
    public class UserInfoController : BaseController
    {
        #region Create required objects
        IUserInfoBLL uBLL = container.Resolve<IUserInfoBLL>();
        IRoleInfoBLL rBLL = container.Resolve<IRoleInfoBLL>();
        IActionInfoBLL aBLL = container.Resolve<IActionInfoBLL>();
        //private static readonly ILog log = LogManager.GetLogger("fresh");
        #endregion

        /// <summary>
        /// GET: Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        ///// <summary>
        ///// GET: UserData
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult UserData(int page, int rows)
        //{
        //    int totalCount = 0;
        //    var userData = uBll.Retrieve(page, rows, out totalCount, u => true, u => u.ID);

        //    return Json(new { total = totalCount, rows = userData });
        //}

        /// <summary>
        /// GET: UserData
        /// </summary>
        /// <returns></returns>
        public ActionResult UserData(int page, int rows, string searchName, string searchRemark)
        {
            int totalCount = 0;
            var userData = !IsNullOrEmpty(true, searchName, searchRemark) ?
            uBLL.Retrieve(page, rows, out totalCount, u => true, u => u.ID) :
            uBLL.Retrieve(page, rows, out totalCount, u => u.UName.Contains(searchName) && u.Remark.Contains(searchRemark), u => u.Sort);
            //uBll.Retrieve(page, rows, out totalCount, u => u.UName.Contains(searchName ?? "impossibleexistsstring:)(:^-^") || u.Remark.Contains(searchRemark ?? "impossibleexistsstring:)(:^-^"), u => u.Sort);

            // Here will get a error :A circular reference was detected while serializing an object of type 'System.Data.Entity.DynamicProxies
            // Solution : new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
            //return Json(new { total = totalCount, rows = userData });
            return Content(JsonConvert.SerializeObject(new { total = totalCount, rows = userData },
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
        }

        /// <summary>
        /// GET: DeleteUser
        /// </summary>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public ActionResult DeleteUser(string userIDs)
        {
            // Retrieve
            string[] idSplit = userIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //var idArray = userIDs.Where(i => i != ',').Select(i => i.ToInt32()); total wrong
            var idArray = idSplit.Select(id => id.ToInt32());
            // Tag
            uBLL.Delete(idArray);
            // Execute
            int effectedRows = uBLL.SaveChanges();

            // Response
            if (effectedRows > 0)
                return Json(new { msg = $"{effectedRows} rows deleted", result = true });
            else
                return Json(new { msg = "Delete failed", result = false });
        }

        /// <summary>
        /// GET: AddUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult AddUser(UserInfo user)
        {
            user.SubTime = DateTime.Now;
            user.ModifiedOn = DateTime.Now;
            uBLL.Create(user);
            bool result = uBLL.SaveChanges() > 0;
            return Json(new { result = result, msg = result ? "Add success" : "Add fail" });
        }

        /// <summary>
        /// GET: AddUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult EditUser(UserInfo user)
        {
            var dbUser = uBLL.Retrieve(u => u.ID == user.ID).FirstOrDefault();
            dbUser.UName = user.UName;
            dbUser.UPwd = user.UPwd;
            dbUser.Remark = user.Remark;

            uBLL.Update(dbUser);
            bool result = uBLL.SaveChanges() > 0;
            return Json(new { result = result, msg = result ? "Edit success" : "Edit fail" });

        }

        /// <summary>
        /// GET: SetRole
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SetRole(int id)
        {
            ViewBag.user = uBLL.Retrieve(u => u.ID == id).FirstOrDefault();
            ViewBag.roles = rBLL.Retrieve(u => true);
            return View();
        }

        [HttpPost]
        public ActionResult SetRole(string roleIDs, int userId)
        {
            IEnumerable<int> idArray = roleIDs.Split(',').Select(i => i.ToInt32());

            // Set role
            if (!string.IsNullOrEmpty(roleIDs) && idArray.Count() > 0)
                uBLL.SetRole(userId, idArray);

            bool result = rBLL.SaveChanges() > 0;
            return JsonNT(new { result = result, msg = result ? "Set role success" : "Set role failed" });
        }

        public ActionResult SetAction(int userId)
        {
            ViewBag.user = uBLL.Retrieve(u => u.ID == userId).First();
            ViewBag.actions = aBLL.Retrieve(a => true);
            return View();
        }

        //// Abandoned
        //[HttpPost]
        //public ActionResult SetAction(string actionData,int userId)
        //{
        //    string[] actions = actionData.Split(',');
        //    //IEnumerable<int> actionIdArray = actionIds.Split(',').Select(id => id.ToInt32());

        //    //if (!string.IsNullOrEmpty(actionIds) && actionIdArray.Count() > 0)
        //    //    uBLL.SetAction(userId, actionIdArray);
        //    return JsonNT("");
        //}

        [HttpPost]
        public ActionResult SetAction(int userId, int actionId, int actionOperate)
        {
            // Set action
            uBLL.SetAction(userId, actionId, actionOperate);

            bool result = uBLL.SaveChanges() > 0;

            // Response
            return JsonNT(new { result = result, msg = result ? "Set action success" : "Set action failed" });
        }

        [HttpPost]
        public ActionResult GetMenu()
        {
            // Potential bug,can not directly use the userinfo from memcached, serialized object lose the navigation property
            // Update userinfo
            //userInfo = uBLL.Retrieve(u => u.ID == userInfo.ID).FirstOrDefault();

            // All of these role's actions
            //IEnumerable<ActionInfo> actions = userInfo.RoleInfoes.Select(r => r.ActionInfoes.Where(a => a.ActionTypeEnum == 1).FirstOrDefault());
            IEnumerable<ActionInfo> roleActions = from role in userInfo.RoleInfoes
                                                  from action in role.ActionInfoes
                                                  where action.ActionTypeEnum == 1
                                                  select action;

            // User's direct action
            //IEnumerable<ActionInfo> userRActions = userInfo.R_UserInfo_ActionInfo.Where(r => r.IsPass && r.ActionInfo.ActionTypeEnum == 1).Select(r=>r.ActionInfo);
            IEnumerable<ActionInfo> userDirectActions = from rua in userInfo.R_UserInfo_ActionInfo
                                                        where rua.IsPass && rua.ActionInfo.ActionTypeEnum == 1
                                                        select rua.ActionInfo;

            // User's direct denied action
            IEnumerable<int> deniedUserActionId = from rua in userInfo.R_UserInfo_ActionInfo
                                                  where !rua.IsPass && rua.ActionInfo.ActionTypeEnum == 1
                                                  select rua.ActionInfoID;

            // User's all allowed action
            IEnumerable<ActionInfo> allActions = from allAction in userDirectActions.Concat(roleActions).Distinct(new ActionInfoEC())
                                                 where !deniedUserActionId.Contains(allAction.ID)
                                                 select allAction;
            return JsonNT(allActions);
        }

        /// <summary>
        /// GET: CauseException
        /// </summary>
        /// <returns></returns>
        public ActionResult CauseException()
        {
            //log.Debug("debug log");
            //return null;
            //int a = 0, b = 0, c = a / b;
            //return JsonNT("");
            throw new Exception("A man made exception");
        }
    }
}

