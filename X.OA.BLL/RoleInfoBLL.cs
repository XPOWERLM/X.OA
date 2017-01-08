using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.OA.IBLL;
using X.OA.Model;

namespace X.OA.BLL
{
    public partial class RoleInfoBLL : BaseBLL<RoleInfo>, IRoleInfoBLL
    {
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="idArray"></param>
        public void Delete(IEnumerable<int> idArray)
        {
            IEnumerable<RoleInfo> roles = Retrieve(u => idArray.Contains(u.ID));
            foreach (RoleInfo role in roles)
                Delete(role);
        }

      
    }
}
