using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.OA.Model;

namespace X.OA.IBLL
{
    public partial interface IRoleInfoBLL : IBaseBll<RoleInfo>
    {
        void Delete(IEnumerable<int> idArray);

        
    }
}
