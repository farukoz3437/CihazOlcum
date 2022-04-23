using Business.Abstract;
using IMandCRM.UI.Models;
using IMandCRM.UI.Identity;
using IMandCRM.UI.Messages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace IMandCRM.UI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        private IAspNetUserService _aspNetUserService;
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IAspNetUserService aspNetUserService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _aspNetUserService = aspNetUserService;
        }
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RoleCreate(string Name)
        {
            if (Name == "")
            {
                TempData["message"] = "Rol adı boş olduğundan rol eklenemedi.|error";
            }
            else
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(Name));
                if (result.Succeeded)
                {
                    TempData["message"] = "Rol başarıyla eklenmiştir.|success";
                }
                else
                {
                    TempData["message"] = "Rol eklerken hata oluştu.|error";
                }
            }

            return RedirectToAction("RoleList");
        }

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonmembers = new List<User>();
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    members.Add(user);
                }
                else
                {
                    nonmembers.Add(user);
                }

            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

            }
            return Redirect("/Admin/RoleEdit/" + model.RoleId);
        }

        public async Task<IActionResult> RoleDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var deleteRole = await _roleManager.DeleteAsync(role);
            if (deleteRole.Succeeded)
            {
                TempData["message"] = "Rol başarıyla silinmiştir.|success";
            }
            else
            {
                TempData["message"] = "Rol silinirken hata oluştu.|error";
            }
            return Redirect("/Admin/RoleList");
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);
                ViewBag.Roles = roles;
                return View(new UserDetailModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber=user.PhoneNumber,
                    IsDelete=user.IsDelete,
                    SelectedRoles = selectedRoles

                });
            }
            return Redirect("/Admin/UserList");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UserEdit(UserDetailModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;
                    user.PhoneNumber = model.PhoneNumber;
                    user.IsDelete = model.IsDelete;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());
                        return Redirect("/Admin/UserList");
                    }
                }
                return View(model);
            }
            else
            {
                TempData["message"] = "Zorunlu alanları doldurunuz.|warning";
            }
            return View(model);
        }

        public async Task<JsonResult> UserDelete(string id)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                var user = await _aspNetUserService.GetById(id);
                if (user.Data == null)
                {
                    alertMessage.ResponseStatus = false;
                    alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                    alertMessage.MessageType = "error";
                    return Json(alertMessage);
                }
                await _aspNetUserService.Delete(user.Data);
                alertMessage.ResponseStatus = true;
                alertMessage.MessageText = "Kayıt başarıyla silindi.";
                alertMessage.MessageType = "success";

                return Json(alertMessage);
            }
            catch (Exception)
            {
                AlertMessage alertMessage = new AlertMessage();
                alertMessage.ResponseStatus = false;
                alertMessage.MessageText = "Kayıt silinirken hata oluştu.";
                alertMessage.MessageType = "error";
                return Json(alertMessage);
            }

        }

        [HttpPost]
        public async Task<JsonResult> UsersDelete(string[] DeleteUsers)
        {
            try
            {
                AlertMessage alertMessage = new AlertMessage();
                foreach (var id in DeleteUsers)
                {
                    var user = await _aspNetUserService.GetById(id);
                    if (user == null)
                    {
                        alertMessage.ResponseStatus = false;
                        alertMessage.MessageText = "Silme işlemi gerçekleştirilemedi.";
                        alertMessage.MessageType = "error";
                        return Json(alertMessage);
                    }
                    await _aspNetUserService.Delete(user.Data);
                }


                alertMessage.ResponseStatus = true;
                alertMessage.MessageText = "Kayıtlar başarıyla silindi.";
                alertMessage.MessageType = "success";

                return Json(alertMessage);
            }
            catch (Exception)
            {
                AlertMessage alertMessage = new AlertMessage();
                alertMessage.ResponseStatus = false;
                alertMessage.MessageText = "Kayıt silinirken hata oluştu.";
                alertMessage.MessageType = "error";
                return Json(alertMessage);

            }

        }

    }
}
