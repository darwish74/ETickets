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
using System.Drawing;
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
        public IActionResult Index(int PageNumber = 1, int PageSize = 6)
        {
            var moviesQuery = _movie.Get(includeprops: e => e.Include(e => e.category).Include(c => c.cinema));
           
            var totalMovies = moviesQuery.Count();
           
            var totalPages = (int)Math.Ceiling((double)totalMovies / PageSize);

            var paginatedMovies = moviesQuery
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            return View(paginatedMovies);
        }

    [Authorize(Roles = "Admin")]
        public IActionResult IndexAdmin(string searchQuery, int PageNumber = 1, int PageSize = 5)
        {
            var movies = _movie.Get(includeprops: e => e.Include(e => e.category).Include(c => c.cinema));

            if (!string.IsNullOrEmpty(searchQuery))
            {
                movies = movies.Where(e => e.Name.ToLower().Contains(searchQuery.ToLower()));
            }

            var totalMovies = movies.Count();
            var totalPages = (int)Math.Ceiling((double)totalMovies / PageSize);

            var paginatedMovies = movies.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = PageNumber;
            ViewBag.SearchQuery = searchQuery;

            return View(paginatedMovies);
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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _movie.GetOne(filter: e => e.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _category.Get();
            ViewBag.Cinemas = _cinema.Get();
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile? file)
        {
            ModelState.Remove("Image");
            ModelState.Remove("Category");
            ModelState.Remove("Cinema");
            ModelState.Remove("ActorMovies");
            ModelState.Remove("Name");

            if (ModelState.IsValid)
            { 
                var existingMovie = _movie.GetOne(filter: e => e.Id == movie.Id);
                if (existingMovie == null)
                {
                    return NotFound();
                }

                existingMovie.Name = movie.Name;
                existingMovie.Description = movie.Description;
                existingMovie.Price = movie.Price;
                existingMovie.StartDate = movie.StartDate;
                existingMovie.EndDate = movie.EndDate;
                existingMovie.CategoryId = movie.CategoryId;
                existingMovie.CinemaId = movie.CinemaId;

                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    if (!string.IsNullOrEmpty(existingMovie.Image))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", existingMovie.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    existingMovie.Image = fileName;
                }

                _movie.Alter(existingMovie);
                _movie.Commit();

                TempData["message"] = "Movie updated successfully.";
                return RedirectToAction("IndexAdmin");
            }

            ViewBag.Categories = _category.Get();
            ViewBag.Cinemas = _cinema.Get();
            return View(movie);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = _movie.GetOne(filter: e => e.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _movie.GetOne(filter: e => e.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            _movie.Delete(movie);
            _movie.Commit();
            TempData["message"] = "Movie deleted successfully.";
            return RedirectToAction("IndexAdmin");
        }


    }
}