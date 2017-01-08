using System.Web.Mvc;
using X.OA.IBLL;
using X.OA.Model;
using X.OA.Web.Properties;
using static X.OA.Common.Helper.MemcacheHelper;
using static X.OA.Common.Helper.UnityHelper;
using Microsoft.Practices.Unity;
using System.Linq;

namespace X.OA.Web.Controllers
{
    /// <summary>
    /// Require user login
    /// </summary>
    public class BaseController : Controller
    {
        #region Create required objects

        public UserInfo userInfo = null;
        IActionInfoBLL aBLL = container.Resolve<IActionInfoBLL>();
        IUserInfoBLL uBLL = container.Resolve<IUserInfoBLL>();
        IR_UserInfo_ActionInfoBLL ruaBLL = container.Resolve<IR_UserInfo_ActionInfoBLL>();

        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region Check login or not

            // Session check
            string sessionId = filterContext.HttpContext.Request.Cookies[Resources.SessionIdName]?.Value;
            if (string.IsNullOrEmpty(sessionId))
            {
                Response.Redirect("/SignIn");
                return;
            }

            // Here may rise a exception : assembly in which the serialized object was declared cannot be found
            // Reason : Lazy load type is 'System.Data.Entity.DynamicProxies.UserInfo_E387...', Assembly: 'EntityFrameworkDynamicProxies...'
            // Solution : Convert it to normal type
            UserInfo result = memcachedClient.Get<UserInfo>(sessionId);
            if (result == null)
            {
                Response.Redirect("/SignIn");
                return;
            }

            // Make sure get the latest userinfo , not the serialized from the memcached.
            else userInfo = uBLL.Retrieve(u => u.ID == result.ID).FirstOrDefault();
            #endregion

            #region Backdoor 
            if (userInfo.UName == "W") return;
            #endregion

            #region Action filter

            string requestUrl = Request.Url.AbsolutePath == "/" ? "home/index" : Request.Url.AbsolutePath.ToLower();

            // Current action
            ActionInfo currentAction = aBLL.Retrieve(a => a.Url == requestUrl).FirstOrDefault();

            // Check current action
            if (currentAction == null)
            {
                Response.Redirect("/Error");
                return;
            }

            // User's direct action
            R_UserInfo_ActionInfo userDirectAction = ruaBLL.Retrieve(rua => rua.ActionInfoID == currentAction.ID).FirstOrDefault();

            // Check user's direct action
            if (userDirectAction != null)
                if (userDirectAction.IsPass) return;
                else
                {
                    Response.Redirect("/Error");
                    return;
                }

            // User's role actions count
            int roleActionCount = (from role in userInfo.RoleInfoes
                                   from action in role.ActionInfoes
                                   where action.ID == currentAction.ID
                                   select action).Count();
            // Check role action
            if (roleActionCount < 1)
            {
                Response.Redirect("/Error");
                return;
            }
            //bool result = userInfo.

            #endregion
            base.OnActionExecuting(filterContext);
        }


        static void Get()
        {

        }

        #region Auxiliary routines

        #endregion
    }
}