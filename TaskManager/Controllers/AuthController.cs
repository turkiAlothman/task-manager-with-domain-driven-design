using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RandomString4Net;
using System.ComponentModel.DataAnnotations;
using Application.Services.Interfaces;
using infrastructure.Extentions;
using TaskManager.Services;
using Domain.Models.Repositories.interfaces;
using TaskManager.RequestForms;
using Domain.Entities;
using Application.Errors;
using Domain.Models.Models;
using TaskManager.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace TaskManager.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInvitesRepository _invitesRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly IResetPasswordRepository _resetPasswordRepository;
        private readonly IAuthService auth;
        private readonly IEmailService _emailService;
        private readonly IInviteService _inviteService;
        private readonly IResetPasswordService _resetPasswordService;
        public AuthController(IEmployeesRepository employeesRepository, IHttpContextAccessor _httpContextAccessor, IInvitesRepository invitesRepository, ITeamsRepository teamsRepository, IAuthService auth, IEmailService emailService, IResetPasswordRepository resetPasswordRepository, IInviteService inviteService, IResetPasswordService resetPasswordService)
        {
            this._employeesRepository = employeesRepository;
            this._httpContextAccessor = _httpContextAccessor;
            this._invitesRepository = invitesRepository;
            this._teamsRepository = teamsRepository;
            this._resetPasswordRepository = resetPasswordRepository;
            this.auth = auth;
            this._emailService = emailService;
            this._inviteService = inviteService;
            this._resetPasswordService = resetPasswordService;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            var UserIdentity = this._httpContextAccessor.HttpContext?.User.Identity;

            if (UserIdentity != null)
            {
                if (UserIdentity.IsAuthenticated)
                    return Redirect("/");
            }

            return View(new LoginForm { returnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> login([FromForm] LoginForm form)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            IError result = await auth.LogInWithCheck(form.Email, form.Password, form.StayLoggedIn);
            if (result != null)
            {
                ModelState.AddModelError("LoginError", "username or password is incorrect");
                return View();
            }

            return LocalRedirect(form.returnUrl);
        }


        public async Task<IActionResult> logOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }




        [HttpGet("/auth/RegisterationRedirect/{email}/{secretKey}")]
        public async Task<IActionResult> RegisterationRedirect(string email, string secretKey)
        {

            Invites invite = await _inviteService.GetInvite(email, secretKey);

            if (invite == null)
                return Unauthorized();


            TempData["email"] = email;
            TempData["secretKey"] = secretKey;


            return RedirectToAction("Register");
        }


        public IActionResult Register()
        {
            string email = (string)TempData["email"];
            string secretKey = (string)TempData["secretKey"];

            if (email.IsNullOrEmpty() || secretKey.IsNullOrEmpty())
                return Unauthorized();

            return View(new RegisterForm { email = email, SecretKey = secretKey });
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterForm registerForm)
        {
            if (!ModelState.IsValid)
                return View(registerForm);

            IError result = await auth.Register(registerForm.FirstName, registerForm.LastName, registerForm.PhoneNumber, registerForm.Position, registerForm.BirthDay, registerForm.Password, registerForm.email, registerForm.SecretKey, registerForm.TeamId);
            if (result != null)
                return StatusCode((int)result.StatusCode);

            return Redirect("/");
        }


        public IActionResult ResetPasswordRequest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordRequest([EmailAddress] string Email)
        {

            IError result = await auth.ResetPasswordRequest(Email, _httpContextAccessor.HttpContext.Request.Host.Value);
            
            if (result != null)
            {
                ModelState.AddModelError("ResetPasswordError", result.Message);
                return View();
            }
            ViewData["Success"] = "email sent suucessfully, please check your email";
            return View();
        }

        [HttpGet("/auth/ResetPasswordRedirect/{email}/{secretKey}")]
        public async Task<IActionResult> ResetPasswordRedirect(string email, string secretKey) {
            
            IError result = await _resetPasswordService.CheckResetPasswordRequest(email, secretKey);

            if (result != null)
                return StatusCode((int) result.StatusCode);


            TempData["email"] = email;
            TempData["secretKey"] = secretKey;
            return RedirectToAction("ResetPassword");

        }
        public IActionResult ResetPassword()
        {
            string email = TempData["email"] as string;
            string secretKey = TempData["secretKey"] as string;

            if (email == null || secretKey == null)
                return Unauthorized();

            return View(new ResetPasswordForm { Email = email , SecretKey = secretKey});
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordForm from)
        {
            if (!ModelState.IsValid)
                return View(from);

            IError result = await auth.ResetPassword(from.Password , from.Email , from.SecretKey);
            if(result != null)
            {
                ModelState.AddModelError("ResetPasswordError", result.Message);
                return View(from);
            }

            return View("/Views/Auth/ResetPasswordSuccess.cshtml");
            
            
        }

        
    }
}
