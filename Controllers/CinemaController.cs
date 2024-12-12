using ETickets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Controllers
{
    public class CinemaController : Controller
    {
        private readonly ApplicationDbContext _Context = new ApplicationDbContext();
        public IActionResult Index(int id)
        {
            var Cinema = _Context.Cinemas.Include(e => e.Movies).ThenInclude(e => e.category).FirstOrDefault(e => e.Id == id);
            return View(Cinema);
        }
        public IActionResult AllCategories()
        {
            var Cinemas = _Context.Cinemas.ToList();
            return View(Cinemas);
        }

    }
}
