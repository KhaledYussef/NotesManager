using Core.Domains.Auth;

using Data.DTOs;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Pal.Services.Email;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }


            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            ApplicationUser user = new()
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);

            }

            return RedirectToAction(nameof(Login));
        }



        // Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }


        #region ForgetPassword
        // ForgetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var email = model.Email;

               

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return View(model);

                }
                

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action("ResetPassword", "Account", new { token, email }, Request.Scheme);

                await _emailService.SendEmail(email, user.FullName, callback, Core.Enums.EmailType.ResetPassword);

                return RedirectToAction(nameof(ForgetPasswordConfirmation));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion
    }

}

