using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> GetAllInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> GetAllIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindAllInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> FindAllIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FindInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> FindIncludeAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        int Count();
        Task<int> CountAsync();
    }
}