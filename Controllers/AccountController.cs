using ETickets.Models;
using ETickets.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {   
            if(!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new("Admin"));
                await  roleManager.CreateAsync(new("User"));
            }
            return View();
        }
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
                    await userManager.AddToRoleAsync(user, "User");
                  
                    await signInManager.SignInAsync(user,false);
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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM LoginVM)
        {
            if (ModelState.IsValid)
            {
             ApplicationUser user=await userManager.FindByNameAsync(LoginVM.Name);
                if (user != null)
                {
                bool found=await userManager.CheckPasswordAsync(user, LoginVM.Password);
                    if (found) 
                    { 
                    await signInManager.SignInAsync(user,LoginVM.RememberMe);
                    return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "UserName Or Password wrong");
            }
            return View();
        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return View("Login");
        }


    }
}
