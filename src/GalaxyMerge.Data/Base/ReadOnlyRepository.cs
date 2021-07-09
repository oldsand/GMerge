using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Base
{
    public abstract class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        private readonly DbContext _context;
        protected readonly DbSet<T> Set;

        protected ReadOnlyRepository(DbContext context)
        {
            _context = context;
            Set = context.Set<T>();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public IEnumerable<T> GetAll()
        {
            return GetQueryable().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetQueryable().ToListAsync();
        }

        public IEnumerable<T> GetAllInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            return GetQueryableIncluding(includeProperties).ToList();
        }

        public async Task<IEnumerable<T>> GetAllIncludeAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            return await GetQueryableIncluding(includeProperties).ToListAsync();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return GetQueryable().Where(predicate).ToList();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetQueryable().Where(predicate).ToListAsync();
        }

        public IEnumerable<T> FindAllInclude(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            return GetQueryableIncluding(includeProperties).Where(predicate).ToList();
        }

        public async Task<IEnumerable<T>> FindAllIncludeAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            return await GetQueryableIncluding(includeProperties).Where(predicate).ToListAsync();
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return GetQueryable().SingleOrDefault(predicate);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetQueryable().SingleOrDefaultAsync(predicate);
        }

        public T FindInclude(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            return GetQueryableIncluding(includeProperties).SingleOrDefault(predicate);
        }

        public async Task<T> FindIncludeAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            return await GetQueryableIncluding(includeProperties).SingleOrDefaultAsync(predicate);
        }

        public int Count()
        {
            return GetQueryable().Count();
        }

        public async Task<int> CountAsync()
        {
            return await GetQueryable().CountAsync();
        }

        protected IQueryable<T> GetQueryable()
        {
            return Set.AsNoTracking();
        }

        protected IQueryable<T> GetQueryableIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            var queryable = Set.AsNoTracking();
            return includeProperties.Aggregate(queryable,
                (current, includeProperty) => current.Include(includeProperty));
        }
    }
}