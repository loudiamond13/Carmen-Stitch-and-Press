using CSP.DAL.DbContexts;
using CSP.Domain.IRepositories.IBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSP.DAL.Repositories.Base
{
    public abstract class GenericRepository<C,T> : IGenericRepository<T> 
        where T : class 
        where C : CarmenStitchAndPressDBContext,
        new()
    {
        protected readonly C _context;

        //private C _entities = new();
        //public C Context 
        //{
        //    get { return _entities; }
        //    set { _entities = value; }
        //}

        protected GenericRepository(C context)
        {
            _context = context;
        }

        public virtual async Task<List<T>> GetAllAsync() 
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? includeBuilder = null) 
        {
            //return _context.Set<T>().FirstOrDefaultAsync(predicate);
            IQueryable<T> query = _context.Set<T>();
            if (includeBuilder != null) 
            {
                query = includeBuilder(query);
            }
            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) 
        {
            //await _context.Set<T>().Where(expression).ToListAsync();
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes) 
            {
                query = query.Include(include);
            }
            return await query.Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(T entity) 
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual async Task SaveAsync() 
        {
            await _context.SaveChangesAsync();
        }

        public virtual void  Delete(T entity) 
        {
             _context.Set<T>().Remove(entity);
        }
    }
}
