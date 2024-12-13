using ETickets.Data;
using ETickets.Models;
using ETickets.Repository;
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
        private readonly ICategory _category;
        private readonly ICinema _cinema;

        public HomeController(IMovie _movie,ICategory _category,ICinema _cinema)
        {
            this._movie = _movie;
            this._category = _category;
            this._cinema = _cinema;
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
        public IActionResult Create()
        {
            var categories = _category.Get().ToList();
            var Cinemas = _cinema.Get().ToList();
            ViewData["categories"] = categories;
            ViewData["Cinemas"] = Cinemas;

            return View(new Movie());
        }

        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile? file)
        {
            ModelState.Remove("Img");
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
               
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                   
                    movie.Image = fileName;
                }

                _movie.Create(movie);

           
                TempData["message"] = "Add product successfuly";

                return RedirectToAction("Index");
            }

            var categories = _category.Get().ToList();
            ViewData["categories"] = categories;
            return View(movie);
        }
        public IActionResult Delete(int MovieId)
        {
            var movie = _movie.GetOne(e => e.Id == MovieId);

            if (movie == null) return Content("NotFound");

            // Delete old img
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", movie.Image);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            _movie.Delete(movie);

            TempData["message"] = "Delete product successfuly";

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
