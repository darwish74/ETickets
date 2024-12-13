using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;

namespace ETickets.Repository
{
    public class ActorRepository : Repository<Actor>, IActor
    {
        public ActorRepository(ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
