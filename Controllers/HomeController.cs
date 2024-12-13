using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ETickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovie _movie;
        public HomeController(IMovie _movie)
        {
            this._movie = _movie;
        }
        public IActionResult Index()
        {
            var Movies= _movie.Get(includeprops: e => e.Include(e => e.category).Include(c => c.cinema)).ToList();  
            return View(Movies);
        }
        public IActionResult Details(int id)
        {
         
            var movie = _movie.GetOne(filter: e => e.Id == id,includeprops: e => e.Include(e => e.category).Include(e => e.cinema).Include(e => e.ActorMovies).ThenInclude(e => e.Actor));
            movie.NoOfViews++;
            _movie.Commit();
            return View(movie);
        }
        public IActionResult Search(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return View();
            }
            
            var movies = _movie.Get(includeprops: e => e.Include(e => e.category).Include(c => c.cinema), filter: a => a.Name.ToLower().Contains(name.ToLower())).ToList();
            return View("Index", movies);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
