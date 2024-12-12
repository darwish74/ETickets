using ETickets.Data;
using ETickets.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
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
        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includeprops = null, bool tracked = true)
        {
            IQueryable<T> query = DbSet;
            if (includeprops != null)
            {
                foreach (var prop in includeprops)
                {
                    query = query.Include(prop);
                }
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

        public T? GetOne(Expression<Func<T, bool>>? filter)
        {
            return Get(filter).FirstOrDefault();    
        }
    }

}
