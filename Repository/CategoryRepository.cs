using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;

namespace ETickets.Repository
{
    public class CategoryRepository : Repository<Category>, ICategory
    {
        public CategoryRepository(ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
