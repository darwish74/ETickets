using ETickets.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Role()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Role(RoleVM role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole IdentityRole = new();
                IdentityRole.Name = role.RoleName;
                var result=await roleManager.CreateAsync(IdentityRole);
                if (result.Succeeded) {
                    ViewBag.success = "Saved Successfully";
                return View();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(role);
        }
    }
}
