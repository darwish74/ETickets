using ETickets.Data;
using ETickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Diagnostics;

namespace ETickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        private readonly ApplicationDbContext _context= new ApplicationDbContext();
        public IActionResult Index()
        {
            var Movies=_context.Movies.Include(c=>c.category).Include(G=>G.cinema).ToList();  
            return View(Movies);
        }
        public IActionResult Details(int id) 
        {
            var movie = _context.Movies
                        .Include(e=>e.category)
                        .Include(c=>c.cinema)
                        .Include(A=>A.ActorMovies)
                        .ThenInclude(A=>A.Actor)
                        .FirstOrDefault(a=>a.Id==id);
            return View(movie);
        }
        public IActionResult Search(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return View();
            }
            var movies = _context.Movies
                        .Include(e=>e.category)
                        .Include(e => e.cinema)
                        .Where(a=>a.Name.ToLower().Contains(name.ToLower())).ToList();
            return View("Index", movies);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
