using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.OA.IBLL;
using X.OA.Model;

namespace X.OA.BLL
{
    public partial class ActionInfoBLL : BaseBLL<ActionInfo>, IActionInfoBLL
    {
        public void SetRole(int actionId, IEnumerable<int> roleIdArray)
        {
            ActionInfo action = Retrieve(a => a.ID == actionId).First();
            if (action == null) return;

            action.RoleInfoes.Clear();
            foreach (int id in roleIdArray)
            {
                RoleInfo role = dbSession.Set<RoleInfo>().Retrieve(r => r.ID == id).First();
                if (role != null)
                    action.RoleInfoes.Add(role);
            }
        }
    }
}
