using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Services;
using Domain.Entities;
using Application.Errors;
using Domain.Models.Repositories.interfaces;
using Application.Errors.Authentication;

namespace TaskManager.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeesRepository _employeesRepository;
        public AuthService(IHttpContextAccessor contextAccessor, IEmployeesRepository employeesRepository)
        {
            _contextAccessor = contextAccessor;
            _employeesRepository = employeesRepository;
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

        public async Task<IError> LogInWithCheck(string email, string password , bool StayLoggedIn)
        {

            Employees employee = await this._employeesRepository.GetByEmailAndPassword(email, password);
            if (employee == null) return new EmailOrPasswordNotCorrectError();
           
            await Login(employee, StayLoggedIn);
            return null;
        }
    }
}
