using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;

namespace ETickets.Repository
{
    public class CinemaRepository : Repository<Cinema>, ICinema
    {
        public CinemaRepository(ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
