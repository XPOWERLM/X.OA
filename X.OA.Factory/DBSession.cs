using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using X.OA.DAL;
using X.OA.IDAL;
using X.OA.Model;

namespace X.OA.Factory
{
    /// <summary>
    /// Manage DAL
    /// </summary>
    /// <typeparam name="TEntity">DbContext type.</typeparam>
    public class DbSession<TEntity> : IDbSession where TEntity : class, new()
    {
        // Why???
        DbContext dbContext = DBContextFactory.CreateDbContext<TEntity>();

        #region Exprire
        //private IUserInfoDAL _UserInfoDal;

        //public IUserInfoDAL UserInfoDal
        //{
        //    get
        //    {
        //        if (_UserInfoDal == null)
        //            // decouple with abstract factory
        //            _UserInfoDal = DALAbstractFactory.CreateInstance<UserInfoDAL>();
        //        return _UserInfoDal;
        //    }
        //    set { _UserInfoDal = value; }
        //}

        #endregion

        // Centrailzation of SaveChanges
        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        private Hashtable set = new Hashtable();

        /// <summary>
        /// Get DAL instance for data model
        /// </summary>
        /// <typeparam name="T">Data model</typeparam>
        /// <returns>DAL instance</returns>
        public IBaseDAL<T> Set<T>() where T : class, new()
        {
            if (set[typeof(T).FullName] == null)
                set.Add(typeof(T).FullName, DALAbstractFactory.CreateInstance<T>());

            return set[typeof(T).FullName] as IBaseDAL<T>;
        }

        public int ExecuteSql(string sql, params SqlParameter[] param)
        {
            return dbContext.Database.ExecuteSqlCommand(sql, param);
        }
    }
}
