using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using netCoreMvcTest.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using netCoreMvcTest.Email;

namespace netCoreMvcTest.Controllers
{

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("create")]
        public async Task<IActionResult> CreateUserAsync()
        {

            var result = await _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = "Anotsu",
                Email = "vadimsadrievtw@gmail.com",
                FirstName = "Alice",
                LastName = "WonderLand"
            }, "password");

            if (result.Succeeded)
            {
                return Content("User was created", "text/html");
            }
            return Content("User creation failed", "text/html");
        }

        // [Authorize]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("private")]
        public IActionResult Private()
        {
            return Content($"Private Hello {User.Identity.Name}");
        }

        [Route("login")]
        public async Task<IActionResult> login(string returnUrl)
        {
            var result = await _signInManager.PasswordSignInAsync("Anotsu", "password", true, true);

            if (result.Succeeded)
            {
                return Content("u are loged in");
            }
            else
            {
                return Content("failed to log in");
            }

        }


        [Route("signout")]
        public async Task<IActionResult> signout()
        {
            //   await HttpContext.Authentication.SignOutAsync(IdentityConstants.ApplicationScheme);

            await _signInManager.SignOutAsync();

            var result = await _signInManager.PasswordSignInAsync("Anotsu", "password", true, true);

            if (result.Succeeded)
            {
                return Content("u signed out!");
            }
            return Content("failed to signout");
        }


        [HttpPost]
        public IActionResult Test()
        {
            return Json(new { value = "test" });
        }
    }
}
