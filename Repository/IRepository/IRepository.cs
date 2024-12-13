using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ETickets.Repository.IRepository
{
    public interface IRepository<T>where T : class
    {
        public void Create(T entity);
        public void Alter(T entity);
        public void Delete(T entity);
        public void Commit();
        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeprops = null, bool tracked = true);
        public T? GetOne(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeprops = null, bool tracked = true);
    }
}
