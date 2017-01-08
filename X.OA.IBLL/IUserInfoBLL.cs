using System.Collections.Generic;
using X.OA.Model;

namespace X.OA.IBLL
{
    public partial interface IUserInfoBLL : IBaseBll<UserInfo>
    {
        void Delete(IEnumerable<int> userInfos);

        void ResetPassword(UserInfo entity);

        void SetRole(int userId, IEnumerable<int> roleIdArray);

        void SetAction(int userId, int actionId, int actionOperate);
    }
    
}
