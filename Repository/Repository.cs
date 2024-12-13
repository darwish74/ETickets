using ETickets.Data;
using ETickets.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ETickets.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext Context;
        private DbSet<T> DbSet; 
        public Repository(ApplicationDbContext _context)
        {
            Context = _context;
            DbSet = Context.Set<T>();
        }
        public void Alter(T entity)
        {
          Context.Update(entity);
        }

        public void Create(T entity)
        {
          Context.Add(entity);
        }

        public void Delete(T entity)
        {
          Context.Remove(entity);
        }
        public void Commit()
        {
            Context.SaveChanges();
        }
        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null,Func<IQueryable<T>,IIncludableQueryable<T,object>> includeprops = null, bool tracked = true)
        {
            IQueryable<T> query = DbSet;
            if (includeprops != null)
            {
            query= includeprops(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if(!tracked)
            {
                query=query.AsNoTracking();
            }
            return query;
           
        }

        public T? GetOne(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeprops = null, bool tracked = true)
        {
            return Get(filter, includeprops).FirstOrDefault();    
        }
    }

}
