using ETickets.Models;
using ETickets.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUserVM userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    UserName = userVM.UserName,
                    Email = userVM.Email,
                    Address = userVM.Address,
                    Name = userVM.Name
                };

                var result = await userManager.CreateAsync(user, userVM.Password);

                if (result.Succeeded)
                {
                    // Optionally, redirect or return success message
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(userVM);
        }


    }
}
