using System;
using System.Linq;
using System.Linq.Expressions;
using X.OA.Factory;

namespace X.OA.IBLL
{
    public interface IBaseBll<T> where T : class, new()
    {
        IDbSession dbSession { get; }

        T Create(T entity);

        IQueryable<T> Retrieve(Expression<Func<T, bool>> whereLambda);

        IQueryable<T> Retrieve<TOrder>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TOrder>> orderLambda, bool isDesc = false);

        void Update(T entity);

        void Delete(T entity);

        int SaveChanges();
    }
}
