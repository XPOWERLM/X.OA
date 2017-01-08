using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using X.OA.IDAL;


namespace X.OA.DAL
{
    
    public class BaseDAL<T> : IBaseDAL<T> where T : class, new()
    {

        DbContext dbContext = DBContextFactory.CreateDbContext<T>();

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Create(T entity) =>
            dbContext.Set<T>().Add(entity);

        /// <summary>
        /// Retrieve entities
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> Retrieve(Expression<Func<T, bool>> whereLambda) =>
            dbContext.Set<T>().Where(whereLambda);

        /// <summary>
        /// Retrieve entities with paging
        /// </summary>
        /// <typeparam name="TOrder">Order field</typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public IQueryable<T> Retrieve<TOrder>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TOrder>> orderLambda, bool isDesc = false)
        {
            DbSet<T> entities = dbContext.Set<T>();
            totalCount = entities.Count();
            return isDesc ? entities.Where(whereLambda).OrderByDescending(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                            entities.Where(whereLambda).OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Update(T entity) =>
            dbContext.Entry(entity).State = EntityState.Modified;


        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Delete(T entity)
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Deleted;
            //dbContext.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        public int SaveChanges() =>
            dbContext.SaveChanges();

    }
}
