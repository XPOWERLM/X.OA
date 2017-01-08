using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace X.OA.Factory
{
    public class DbSessionFactoty
    {

        public static IDbSession CreateDbSession<T>() where T : class, new()
        {
            IDbSession dbSession = (IDbSession)CallContext.GetData("dbSession");
            if (dbSession == null)
            {
                dbSession = new DbSession<T>();
                CallContext.SetData("dbSession", dbSession);
            }
            return dbSession;
        }
    }
}
