using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.OA.IDAL;

namespace X.OA.Factory
{
    public interface IDbSession
    {
        //IUserInfoDAL UserInfoDal { get; set; }

        int SaveChanges();

        IBaseDAL<T> Set<T>() where T : class, new();

        int ExecuteSql(string sql, params SqlParameter[] param);
    }
}
