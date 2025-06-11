using AspNetCoreMVC_SchoolSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AspNetCoreMVC_SchoolSystem.Controllers {
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller {
        RoleManager<IdentityRole> _roleManager;
        UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }
        public IActionResult Index() {
            return View(_roleManager.Roles.OrderBy(role => role.Name));
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name) {
            if (ModelState.IsValid) {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    AddIdentityErrors(result);
                }
            }
            return View(name);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id) {
            IdentityRole roleToDelete = await _roleManager.FindByIdAsync(id);
            if (roleToDelete != null) {
                var result = await _roleManager.DeleteAsync(roleToDelete);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    AddIdentityErrors(result);
                }
            }
            ModelState.AddModelError("", "Role not found");
            return View("Index", _roleManager.Roles);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id) {
            IdentityRole roleToEdit = await _roleManager.FindByIdAsync(id);
            List <AppUser> members = new List<AppUser>();
            List <AppUser> nonMembers = new List<AppUser>();
            var allUsers = _userManager.Users.ToList();
            foreach (var user in allUsers) {
                var list = await _userManager.IsInRoleAsync(user, roleToEdit.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleState {
                Members= members,
                NonMembers= nonMembers,
                Role = roleToEdit,
            });
        }
        public async Task<IActionResult> GetToDelete(string id) {
            var studentDetails = await _roleManager.FindByIdAsync(id);
            return View(studentDetails);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(RoleModification roleModification) {
            if (ModelState.IsValid) {
                foreach (string userId in roleModification.AddIds ?? new string[] { }) {
                    AppUser userToAdd = await _userManager.FindByIdAsync(userId);
                    if (userToAdd != null) {
                        IdentityResult result = await _userManager.AddToRoleAsync(userToAdd, roleModification.RoleName);
                        if (!result.Succeeded) {
                            AddIdentityErrors(result);

                        }
                    }
                }
                foreach (string userId in roleModification.DeleteIds ?? new string[] { }) {
                    AppUser userToDelete = await _userManager.FindByIdAsync(userId);
                    if (userToDelete != null) {
                        IdentityResult result = await _userManager.RemoveFromRoleAsync
                            (userToDelete, roleModification.RoleName);
                        if (!result.Succeeded) {
                            AddIdentityErrors(result);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Špatně zadaná změna, zkontroluj údaje");
            return RedirectToAction("Index");
        }

        private void AddIdentityErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
