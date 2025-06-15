using AspNetCoreMVC_SchoolSystem.Models;
using AspNetCoreMVC_SchoolSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC_SchoolSystem.Controllers {
    [Authorize]
    public class AccountController : Controller {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl) {
            LoginViewModel model = new LoginViewModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login) {
            if (ModelState.IsValid) {
                AppUser userToLogin = await _userManager.FindByNameAsync(login.UserName);
                if (userToLogin != null) {
                    var signInResult = await _signInManager.PasswordSignInAsync(userToLogin, login.Password, login.Remember, false);
                    if (signInResult.Succeeded) {
                        return Redirect(login.ReturnUrl ?? "/");
                    }
                }
            }
            ModelState.AddModelError("", "Uživatel nenalezen nebo neplatné heslo");
            login.Password = string.Empty;
            return View(login);
        }
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied() {
            return View();
        }
    }
}
