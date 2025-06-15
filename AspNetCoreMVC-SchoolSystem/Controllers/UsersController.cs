using AspNetCoreMVC_SchoolSystem.Models;
using AspNetCoreMVC_SchoolSystem.Services;
using AspNetCoreMVC_SchoolSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC_SchoolSystem.Controllers {
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly List<string> _protectedUserIds;

        public UsersController(
            UserManager<AppUser> userManager,
            IPasswordHasher<AppUser> passwordHasher,
            IPasswordValidator<AppUser> passwordValidator,
            IConfiguration configuration) {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _protectedUserIds = configuration.GetSection("ProtectedUsers").Get<List<string>>();
        }

        public IActionResult Index() {
            ViewBag.ProtectedUserIds = _protectedUserIds;
            return View(_userManager.Users);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserViewModel newUser) {
            if (ModelState.IsValid) {
                AppUser userToAdd = new AppUser {
                    UserName = newUser.Name,
                    Email = newUser.Email,
                };
                IdentityResult result = await _userManager.CreateAsync(userToAdd, newUser.Password);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(newUser);
        }
        public async Task<IActionResult> EditAsync(string id) {
            var userToEdit = await _userManager.FindByIdAsync(id);
            if (userToEdit == null) {
                return View("NotFound");
            }
            return View(userToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, string email, string password) {
            AppUser userToEdit = await _userManager.FindByIdAsync(id);
            if (userToEdit == null) {
                return View("NotFound");
            }

            if (string.IsNullOrEmpty(email)) {
                ModelState.AddModelError("", "E-mail nesmí být prázdný");
            }
            else {
                userToEdit.Email = email;
            }

            IdentityResult validPass = IdentityResult.Success;

            if (string.IsNullOrEmpty(password)) {
                ModelState.AddModelError("", "Heslo nesmí být prázdné");
            }
            else {
                validPass = await _passwordValidator.ValidateAsync(_userManager, userToEdit, password);
                if (validPass.Succeeded) {
                    userToEdit.PasswordHash = _passwordHasher.HashPassword(userToEdit, password);
                }
                else {
                    AddIdentityErrors(validPass);
                }
            }

            if (ModelState.IsValid) {
                IdentityResult result = await _userManager.UpdateAsync(userToEdit);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    AddIdentityErrors(result);
                }
            }

            return View(userToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id) {
            if (_protectedUserIds.Contains(id)) {
                ModelState.AddModelError("", "Tento uživatel nelze smazat.");
                return RedirectToAction("Index");
            }

            AppUser userToDelete = await _userManager.FindByIdAsync(id);

            if (userToDelete != null) {
                IdentityResult result = await _userManager.DeleteAsync(userToDelete);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    AddIdentityErrors(result);
                }
            }
            else {
                ModelState.AddModelError("", "Uživatel nenalezen");
            }

            return View("Index", _userManager.Users);
        }

        private void AddIdentityErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }
        public async Task<IActionResult> GetToDelete(string id) {
            var studentDetails = await _userManager.FindByIdAsync(id);
            return View(studentDetails);
        }
    }
}
