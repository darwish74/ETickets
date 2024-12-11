using ETickets.Data;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _Context=new ApplicationDbContext();  
        public IActionResult Index(int id)
        {
            var actor= _Context.Actors.Find(id);
            return View(actor);
        }
    }
}
