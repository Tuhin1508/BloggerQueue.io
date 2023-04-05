using ChemBlog2._0.Data;
using ChemBlog2._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChemBlog2._0.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Login(string URL = null)
        {
            ViewData["PageURL"] = URL;
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string URL)
        {
            if (string.IsNullOrEmpty(URL))
            {
                URL = "/Home/Index";
            }
            var user = new ApplicationUser() { UserName = model.UserName };
            await _signInManager.SignInAsync(user, isPersistent: false);
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect(URL);
            }
            ViewBag.ErrorMessage = "Please Complete Your Registraion";
            return View(ViewBag);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel Model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = Model.Name , Email = Model.EmialAddress, Name= Model.Name};
                var result = await _userManager.CreateAsync(user, Model.Password);
                if(result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
    }
}
