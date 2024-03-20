using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.ExtentionsMethods;
using TaskManager.Models.DomainModels;
using TaskManager.Models.Repositories.interfaces;
using TaskManager.RequestForms;
using TaskManager.Services.Interfaces;


namespace TaskManager.Services.Implementers
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public async Task Login(Employees employee, bool StayLoggedIn = true)
        {
            List<Claim> claims = new List<Claim> {
                {new Claim(ClaimTypes.Email,employee.Email)},
                {new Claim(ClaimTypes.NameIdentifier,employee.Id.ToString())},
                {new Claim("FirstName",employee.FirstName)},
                {new Claim("LastName",employee.LastName)},
                {new Claim("position",employee.Position)},
                {new Claim("manager",employee.Manager.ToString())}
                 };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = StayLoggedIn });

        }
    }
}
