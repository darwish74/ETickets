using ETickets.Data;
using ETickets.Models;
using ETickets.Repository;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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

        public HomeController(IMovie _movie, ICategory _category, ICinema _cinema)
        {
            this._movie = _movie;
            this._category = _category;
            this._cinema = _cinema;
        }
        public IActionResult Index()
        {
            var Movies = _movie.Get(includeprops: e => e.Include(e => e.category).Include(c => c.cinema)).ToList();
            return View(Movies);
        }
        [Authorize(Roles ="Admin")]
        public IActionResult IndexAdmin()
        {
            var Movies = _movie.Get(includeprops: e => e.Include(e => e.category).Include(c => c.cinema)).ToList();
            return View(Movies);
        }
        public IActionResult Details(int id)
        {

            var movie = _movie.GetOne(filter: e => e.Id == id, includeprops: e => e.Include(e => e.category).Include(e => e.cinema).Include(e => e.ActorMovies).ThenInclude(e => e.Actor));
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
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _category.Get();
            ViewBag.Cinemas = _cinema.Get();
            return View(new Movie());
        }
      
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile? file)
        {
                ModelState.Remove("Image");
                ModelState.Remove("Category");
                ModelState.Remove("Cinema");
                ModelState.Remove("ActorMovies");

                if (ModelState.IsValid)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);
                        }

                        movie.Image = fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("Image", "Please upload an image.");
                        ViewBag.Categories = _category.Get();
                        ViewBag.Cinemas = _cinema.Get();
                        return View(movie);
                    }

                    _movie.Create(movie);
                    _movie.Commit();
                    TempData["message"] = "Add Movie successfully";
                    return RedirectToAction("Index");
                }

                ViewBag.Categories = _category.Get();
                ViewBag.Cinemas = _cinema.Get();
                return View(movie);
        }
        public IActionResult Edit(int id)
        {
           return  View();
        }

        }
    }