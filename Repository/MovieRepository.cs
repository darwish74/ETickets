using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;

namespace ETickets.Repository
{
    public class MovieRepository : Repository<Movie>, IMovie
    {
        public MovieRepository(ApplicationDbContext _context) : base(_context)
        {

        }
    }
}
