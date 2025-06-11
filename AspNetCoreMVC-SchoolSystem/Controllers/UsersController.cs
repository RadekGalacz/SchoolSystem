using AspNetCoreMVC_SchoolSystem.Models;
using AspNetCoreMVC_SchoolSystem.Services;
using AspNetCoreMVC_SchoolSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC_SchoolSystem.Controllers {
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller {
        UserManager<AppUser> _userManager;
        IPasswordHasher<AppUser> _passwordHasher;
        IPasswordValidator<AppUser> _passwordValidator;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator ) {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
        }

        public IActionResult Index() {
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
                ModelState.AddModelError("", "E-mail cannot be empty");
            }
            else {
                userToEdit.Email = email;
            }

            IdentityResult validPass = IdentityResult.Success;

            if (string.IsNullOrEmpty(password)) {
                ModelState.AddModelError("", "Password cannot be empty");
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
                ModelState.AddModelError("", "User not found");
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
