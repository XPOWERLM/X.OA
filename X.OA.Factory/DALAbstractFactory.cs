using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using X.OA.DAL;
using X.OA.Factory.Properties;
using X.OA.IDAL;

namespace X.OA.Factory
{
    public class DALAbstractFactory
    {
        private static readonly string DALNameSpace = ConfigurationManager.AppSettings[Resources.Setting_DALNameSpace];
        private static readonly string DALAssembly = ConfigurationManager.AppSettings[Resources.Setting_DALAssembly];
        #region Expire


        //public static IUserInfoDAL CreateUserInfoDAL()
        //{
        //    string fullName = $"{DALNameSpace}.UserInfoDAL";
        //    return CreateInstance<UserInfoDAL>(fullName, DALAssembly);
        //} 
        #endregion

        //public static T CreateInstance<T>() where T : class, new() =>
        //    Assembly.Load(typeof(T).Assembly.FullName).CreateInstance(typeof(T).FullName) as T;

        /// <summary>
        /// Create DAL instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>DAL</returns>
        public static object CreateInstance<T>() where T : class, new() =>
            Assembly.Load(DALAssembly).CreateInstance($"{DALNameSpace}.{typeof(T).Name}DAL");
    }
}
