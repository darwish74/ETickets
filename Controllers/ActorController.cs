using ETickets.Data;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActor _actor;
        public ActorController(IActor _actor)
        {
            this._actor = _actor;
        }
        public IActionResult Index(int id)
        {
            var actor = _actor.GetOne(e=>e.Id==id);
            return View(actor);
        }
    }
}
