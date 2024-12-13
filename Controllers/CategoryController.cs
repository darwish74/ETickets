using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategory _category;  
        //public IActionResult Index(int id)
        //{
        //    var Category = _Context.Categories.Include(e => e.Movies).ThenInclude(e => e.cinema).FirstOrDefault(e => e.Id == id);
        //    return View(Category);
        public CategoryController(ICategory _category)
        {
            this._category = _category;
        }
        public IActionResult Index(int id)
        {
            var Category = _category.GetOne(filter:e => e.Id == id, e => e.Include(e => e.Movies).ThenInclude(e => e.cinema));
            return View(Category);
        }

        public IActionResult AllCategories()
        {
            var Categories = _category.Get().ToList();
            return View(Categories);
        }
    }
}
