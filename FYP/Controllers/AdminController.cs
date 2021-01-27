using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    public class AdminController : Controller
    {
        private readonly FYPContext _context;
        private UserManager<IdentityUser> _userManager;

        public AdminController(FYPContext context, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authorize()
        {
            return View();
        }

        public IActionResult ViewUser()
        {
            return View();
        }

      [ActionName("CreateUser")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ActionName("CreateUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUserAsync(NewUserViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid) {
                model.admin = _userManager.GetUserId(User);
                var newUser = new IdentityUser { UserName = model.email, Email = model.email };
                var result = await _userManager.CreateAsync(newUser, model.password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "User");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                ViewBag.Success = "A New User has been created!";
            }

            
            return View();
        }
    }
}
