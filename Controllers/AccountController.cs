using ETickets.Models;
using ETickets.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ETickets.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new("Admin"));
                await roleManager.CreateAsync(new("User"));
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
                    await userManager.AddToRoleAsync(user, "Admin");

                    await signInManager.SignInAsync(user, false);
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
                ApplicationUser user = await userManager.FindByNameAsync(LoginVM.Name);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, LoginVM.Password);
                    if (found)
                    {
                        await signInManager.SignInAsync(user, LoginVM.RememberMe);
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
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            return View(new ApplicationUserVM()
            {
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Address = user.Address
            });
        }
        [HttpPost]
        public async Task<IActionResult> Profile(ApplicationUserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User); 
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(model);
                }
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Name = model.Name;
                user.Address = model.Address;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["success"] = "Profile updated successfully.";
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> updatePhoto(IFormFile photo)
        {
            var user = await userManager.GetUserAsync(User);

            if (photo != null && photo.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Profile", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    photo.CopyTo(stream);
                }
                user.Photo = fileName;
                await userManager.UpdateAsync(user);
            }

            TempData["success"] = "Profile photo updated successfully";

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    await signInManager.RefreshSignInAsync(user);
                    TempData["success"] = "Your password has been changed successfully.";
                    return RedirectToAction("Profile", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var users = await userManager.Users.Where(u => u.Email == model.Email).ToListAsync();
                if (users.Count == 0)
                {
                    ModelState.AddModelError("", "No user found with this email.");
                    return View(model);
                }
                else if (users.Count > 1)
                {
                    ModelState.AddModelError("", "Multiple users found with this email. Please contact support.");
                    return View(model);
                }

                var user = users.First();
                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);
                    TempData["Message"] = "A password reset link has been sent to your email.";
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Email not found.");
            }
            return View(model);
        }

    }
}
