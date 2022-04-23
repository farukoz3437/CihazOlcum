using Business.Abstract;
using Core.Utilities.Result;
using Entities.Concrete;
using IMandCRM.UI.EmailServices;
using IMandCRM.UI.HelperMethods;
using IMandCRM.UI.Identity;
using IMandCRM.UI.Messages;
using IMandCRM.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel() { ReturnUrl = ReturnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı ile daha önce hesap oluşturulmamış.");
                return View(model);
            }
            if(user.IsDelete == true)
            {
                ModelState.AddModelError("", "Bu kullanıcı silinmiş.");
                return View(model);
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen email hesabınıza gelen link ile üyeliğinizi onaylayınız.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                ViewData.Add("UserEmail", user.Email);
                return Redirect(model.ReturnUrl ?? "~/Home/Index");
            }

            ModelState.AddModelError("", "Girilen kullanıcı adı veya parola yanlış.");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, IFormFile UserPhoto)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsDelete = false
            };
            if (UserPhoto != null)
            {
                user.UserPhoto = await ImageUpload.Upload(UserPhoto, "wwwroot\\assets\\media\\users");
            }
            else
            {
                user.UserPhoto = Constants.Constants.DefaultUserPhoto;
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = code });

                //email ile token gönder
                string subject = "Hesabınızı onaylayınız";
                string htmlMessage = $"Lütfen email hesabınızı onaylamak için linke <a href='{Constants.Constants.Url}{url}'><b>tıklayınız.<b></a>";
                await _emailSender.SendEmailAsync(model.Email, subject, htmlMessage);
                TempData["message"] = "Maildeki linke tıklayarak üyelik onaylanması gerekmektedir.|success";
                return RedirectToAction("Register", "Account");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);

        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {

            if (userId == null || token == null)
            {
                CreateMessage("Geçersiz token yok", "danger");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    CreateMessage("Hesabınız onaylandı", "success");
                    return View();
                }
            }
            CreateMessage("Hesabınız onaylanmadı", "warning");
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                CreateMessage("Mail adresi giriniz.", "warning");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                CreateMessage("Geçersiz kullanıcı.", "warning");
                return View();
            }
            //generate token
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = code, email = user.Email });

            //email ile token gönder
            string subject = "Reset Password";
            string htmlMessage = $"Parolanızı yenilemek için linke <a href='{Constants.Constants.Url}{url}'><b>tıklayınız.<b></a>";
            await _emailSender.SendEmailAsync(model.Email, subject, htmlMessage);
            CreateMessage("Şifre sıfırlamak için mailinizi kontrol ediniz.", "success");
            return View();
        }

        public IActionResult ResetPassword(string userId, string token, string email)
        {
            if (userId == null || token == null)
            {
                CreateMessage("Geçersiz bilgi.", "warn");
                return RedirectToAction("Login", "Account");
            }
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                CreateMessage("Geçersiz kullanıcı.", "warn");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                CreateMessage("Parolanız başarıyla sıfırlanmıştır.", "success");
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", result.Errors.ToList().FirstOrDefault().Description);
            return View(model);
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void CreateMessage(string message, string alertType)
        {
            var msg = new AlertMessage()
            {
                MessageText = message,
                MessageType = alertType
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }


    }

}
