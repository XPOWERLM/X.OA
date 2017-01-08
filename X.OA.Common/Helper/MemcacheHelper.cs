using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.OA.Common.Helper
{
    public static class MemcacheHelper
    {
        public static MemcachedClient memcachedClient = null;
        static MemcacheHelper()
        {
            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            string[] clients = ConfigurationManager.AppSettings["MemcachedClients"].Split('&');
            foreach (string client in clients)
                config.AddServer(client);
            memcachedClient = new MemcachedClient(config);
        }
    }
}
