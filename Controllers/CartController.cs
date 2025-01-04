using ETickets.Models;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class CartController : Controller
    {
        private readonly ICart _cart;
        private readonly UserManager<ApplicationUser> userManager;

        public CartController(ICart cart,UserManager<ApplicationUser> userManager)
        {
            _cart = cart;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var carts = _cart.Get();
            return View(carts);
        }
        [HttpPost]
        public IActionResult BookTicket(int TicketCount,int MovieId)
        {
            var userId = userManager.GetUserId(User);
            if(userId!=null)
            {
                _cart.Create(new()
                {
                    ApplicationUserId = userId,
                    MovieId= MovieId,
                    Count= TicketCount,
                    time=DateTime.Now
                });
                _cart.Commit(); 
                TempData["success"] = "Ticket has been booked and Add TO cart Successfully";
                return RedirectToAction("index","home");
            }
                return RedirectToAction("Login", "Account");
        }

    }
}
