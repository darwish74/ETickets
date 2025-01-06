using ETickets.Models;
using ETickets.Repository.IRepository;
using ETickets.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace ETickets.Controllers
{
    public class CartController : Controller
    {
        private readonly ICart _cart;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        public CartController(ICart cart,UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _cart = cart;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }
        public IActionResult Index()
        {
            var carts = _cart.Get(includeprops:e=>e.Include(e=>e.Movie),filter: e => e.ApplicationUserId == userManager.GetUserId(User)).ToList();
            ViewBag.Total = carts.Sum(c => c.Movie.Price * c.Count);
            return View(carts);
        }
        [HttpPost]
        public IActionResult BookTicket(int TicketCount,int MovieId)
        {
            var userId = userManager.GetUserId(User);

            if(userId!=null)
            {
                var item=_cart.Get(filter: e => e.ApplicationUserId == userId && e.MovieId== MovieId).FirstOrDefault();
                if (item != null)
                {
                    item.Count += TicketCount;
                    _cart.Commit();
                }
                else
                {
                    _cart.Create(new()
                    {
                        ApplicationUserId = userId,
                        MovieId = MovieId,
                        Count = TicketCount,
                        time = DateTime.Now
                    });
                    _cart.Commit();
                }
                   
                    TempData["success"] = "Ticket has been booked and Add TO cart Successfully";
                    return RedirectToAction("index", "home");
            }
                    return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int itemId, string action)
        {
            var cartItem = _cart.GetOne(c => c.MovieId == itemId);
            if (cartItem != null)
            {
                if (action == "increment")
                {
                    cartItem.Count++;
                    _cart.Commit();
                }
                else if (action == "decrement" && cartItem.Count > 1)
                {
                    cartItem.Count--;
                }
                _cart.Commit();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int itemId)
        {
            var cartItem = _cart.GetOne(c => c.MovieId == itemId);
            if (cartItem != null)
            {
                _cart.Delete(cartItem);
                _cart.Commit();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Pay()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Cart/PaymentSuccess",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Cart/PaymentCancel",
            };
            var carts = _cart.Get(includeprops: e => e.Include(e => e.Movie), filter: e => e.ApplicationUserId == userManager.GetUserId(User)).ToList();
            foreach (var item in carts)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Movie.Name,
                        },
                        UnitAmount = (long)item.Movie.Price * 100,
                    },
                    Quantity = item.Count,
                });
            }
            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
        public async Task<IActionResult> PaymentSuccess()
        {
            var user = await userManager.GetUserAsync(User);
            await emailSender.SendEmailAsync(await userManager.GetEmailAsync(user), "success pay", $"<html><body> <h1>   Your payment was successful! Thank you for your purchase.</h1> </body></html>");
            return RedirectToAction("Index", "Home");
         
        }
     
        public async Task<IActionResult> PaymentCancel()
        {
            var user = await userManager.GetUserAsync(User);
            await emailSender.SendEmailAsync(await userManager.GetEmailAsync(user), "failed pay", $"<html><body> <h1>Your payment was canceled. You can try again or contact support.</h1> </body></html>");
            return RedirectToAction("Index", "Home");
        }
    }
}
