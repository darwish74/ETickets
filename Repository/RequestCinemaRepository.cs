using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;

namespace ETickets.Repository
{
    public class RequestCinemaRepository : Repository<RequestCinema>,IRequestsCinemaRepository
    {
        public RequestCinemaRepository(ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
