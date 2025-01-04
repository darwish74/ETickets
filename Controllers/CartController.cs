using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class CartController : Controller
    {
        private readonly ICart _cart;

        public CartController(ICart cart)
        {
            _cart = cart;
        }
        public IActionResult Index()
        {
            var carts = _cart.Get();
            return View(carts);
        }
    }
}
