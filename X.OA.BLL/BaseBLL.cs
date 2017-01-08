using System;
using System.Linq;
using System.Linq.Expressions;
using X.OA.Factory;
using X.OA.IBLL;

namespace X.OA.BLL
{
    public class BaseBLL<T> : IBaseBll<T> where T : class, new()
    {
        #region Another way 
        //public BaseBLL()
        //{
        //    SetCurrentDAL();
        //}

        //public IBaseDAL<T> currentDAL;

        //public abstract void SetCurrentDAL(); 
        #endregion

        public IDbSession dbSession
        {
            get
            {
                return DbSessionFactoty.CreateDbSession<T>();
            }
        }

        public T Create(T entity) =>
          dbSession.Set<T>().Create(entity);

        public IQueryable<T> Retrieve(Expression<Func<T, bool>> whereLambda) =>
            dbSession.Set<T>().Retrieve(whereLambda);

        public IQueryable<T> Retrieve<TOrder>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TOrder>> orderLambda, bool isDesc = false) =>
            dbSession.Set<T>().Retrieve(pageIndex, pageSize, out totalCount, whereLambda, orderLambda, isDesc);

        public void Update(T entity) =>
            dbSession.Set<T>().Update(entity);

        public void Delete(T entity) =>
            dbSession.Set<T>().Delete(entity);

        public int SaveChanges() =>
            dbSession.Set<T>().SaveChanges();

    }
}
