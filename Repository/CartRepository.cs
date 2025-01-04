using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;

namespace ETickets.Repository
{
    public class CartRepository : Repository<Cart>,ICart
    {
        public CartRepository(ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
