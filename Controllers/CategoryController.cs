using ETickets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _Context=new ApplicationDbContext();  
        public IActionResult Index(int id)
        {
            var Category = _Context.Categories.Include(e => e.Movies).ThenInclude(e => e.cinema).FirstOrDefault(e => e.Id == id);
            return View(Category);
        
        }
        public IActionResult AllCategories()
        { 
            var Categories=_Context.Categories.ToList();    
              return View(Categories);
        }
    }
}
