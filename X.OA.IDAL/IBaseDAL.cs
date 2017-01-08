using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace X.OA.IDAL
{
    public interface IBaseDAL<T> where T : class, new()
    {

        T Create(T entity);

        IQueryable<T> Retrieve(Expression<Func<T, bool>> whereLambda);

        IQueryable<T> Retrieve<TOrder>(int pageIndex,int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TOrder>> orderLambda, bool isDesc = false);

        void Update(T entity);

        void Delete(T entity);

        int SaveChanges();
    }
}
