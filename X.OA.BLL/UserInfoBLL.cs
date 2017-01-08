using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mail;
using X.OA.Common.Utility;
using X.OA.IBLL;
using X.OA.IDAL;
using X.OA.Model;

namespace X.OA.BLL
{
    public partial class UserInfoBLL : BaseBLL<UserInfo>, IUserInfoBLL
    {
        public void Delete(IEnumerable<int> userInfoIds)
        {
            //var userInfoEntities = Retrieve(u => userInfos.Any(user => user.ID == u.ID));
            var userInfos = Retrieve(u => userInfoIds.Contains(u.ID));
            foreach (UserInfo userInfo in userInfos)
                Delete(userInfo);
        }

        /// <summary>
        /// Reset user password.
        /// Abandoned
        /// </summary>
        /// <param name="entity">Require correct UName and Mail</param>
        public void ResetPassword(UserInfo entity)
        {
            // Generate new password and reset it
            string newPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
            var retrieveResult = Retrieve(u => u.UName == entity.UName && u.Remark == entity.Remark).FirstOrDefault();
            retrieveResult.UPwd = newPassword;
            Update(retrieveResult);
            SaveChanges();

            // Build email
            MailMessage message = new MailMessage();
            message.To = "someone@outlook.com";
            message.Subject = "Reset your password";
            message.Body = $"Here is your new password: {newPassword}";

            // Send email
            MailUtility.SendMail(message);
        }

        /// <summary>
        /// Set role
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <param name="idArray">Role id</param>
        /// <returns></returns>
        public void SetRole(int userId, IEnumerable<int> idArray)
        {
            UserInfo user = dbSession.Set<UserInfo>().Retrieve(u => u.ID == userId).First();
            if (user == null) return;

            user.RoleInfoes.Clear();
            foreach (int id in idArray)
            {
                RoleInfo role = dbSession.Set<RoleInfo>().Retrieve(r => r.ID == id).First();
                if (role != null)
                    user.RoleInfoes.Add(role);
            }

        }

        /// <summary>
        /// Allocate action for user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="actionId">Action id</param>
        /// <param name="actionOperate">0:Clear,1:Grant,2:Denied</param>
        public void SetAction(int userId, int actionId, int actionOperate)
        {
            // Retrieve
            R_UserInfo_ActionInfo rua = dbSession.Set<R_UserInfo_ActionInfo>().Retrieve(r => r.UserInfoID == userId && r.ActionInfoID == actionId).FirstOrDefault();

            // Delete
            if (actionOperate == 0)
                if (rua != null) dbSession.Set<R_UserInfo_ActionInfo>().Delete(rua);

            // Grant or denied
            if (actionOperate == 1 || actionOperate == 2)
            {
                // Create
                if (rua == null)
                {
                    rua = new R_UserInfo_ActionInfo
                    {
                        ActionInfoID = actionId,
                        UserInfoID = userId,
                        IsPass = ((Func<bool>)delegate
                        {
                            if (actionOperate == 1) return true;
                            return false;
                        }).Invoke()
                    };
                    dbSession.Set<R_UserInfo_ActionInfo>().Create(rua);
                }
                else
                    // Update
                    dbSession.Set<R_UserInfo_ActionInfo>().Update(rua);
            }
        }



    }
}
