using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.OA.Common.Helper
{
    public static class RedisHelper
    {
        static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["Redis-ServerAddress"]);
        public static IDatabase redisDB = redis.GetDatabase();
    }
}
