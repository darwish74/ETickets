using System.Linq.Expressions;

namespace ETickets.Repository.IRepository
{
    public interface IRepository<T>where T : class
    {
        public void Create(T entity);
        public void Alter(T entity);
        public void Delete(T entity);
        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]?includeprops=null,bool tracked=true);
        public T? GetOne(Expression<Func<T, bool>>? filter);
    }
}
