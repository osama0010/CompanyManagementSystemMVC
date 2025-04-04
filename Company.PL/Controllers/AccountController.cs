using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSettings emailSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IEmailSettings emailSettings)
        {
            _signInManager = signInManager;
            this.emailSettings = emailSettings;
            _userManager = userManager;
        }


        // Register
        // baseUrl/Account/Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) // Server-Side Validation
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    FName = model.FName,
                    LName = model.LName,
                    Email = model.Email,
                    IsAgree = model.IsAgree,
                };

                var result = await _userManager.CreateAsync(User, model.Password);
                if(result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect Password");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email does not exist");
                }
            }
            return View(model);
        }

        // Sign Out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        // ForgetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Send Email

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    // Valid For One User and One Time

                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = user.Email, Token = token }, Request.Scheme);

                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        To = model.Email,
                        Body = ResetPasswordLink
                    };

                    emailSettings.SendMail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email Not Found");
                }
            }
                return View("ForgetPassword", model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        // Reset Password

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
    }
}
