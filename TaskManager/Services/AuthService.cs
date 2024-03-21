using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Services;
using Domain.Entities;


namespace TaskManager.Services
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
