using ETickets.Data;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Controllers
{
    public class CinemaController : Controller
    {
        private readonly ICinema _cinemaRepository;

        public CinemaController(ICinema cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }
        public IActionResult Index(int id)
        {
            var Cinema = _cinemaRepository.Get(includeprops:e=>e.Include(e=>e.Movies)).ToList();
            return View(Cinema);
        }
        public IActionResult CinemaMovies(int id)
        {
            var Movies = _cinemaRepository.Get(filter:e=>e.Id==id,includeprops:e => e.Include(e => e.Movies)).ToList();
            return View(Movies); 
        }

    }
}
