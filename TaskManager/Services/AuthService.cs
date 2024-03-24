using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Services;
using Domain.Entities;
using Application.Errors;
using Domain.Models.Repositories.interfaces;
using Application.Errors.Authentication;
using infrastructure.Persistence.Repositores;
using TaskManager.RequestForms;
using Application.Services.Interfaces;
using infrastructure.Extentions;
using Application.Errors.Invites;
using Application.Errors.Teams;

namespace TaskManager.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IInvitesRepository _invitesRepository;
        private readonly ITeamsRepository _teamsRepository;
        public AuthService(IHttpContextAccessor contextAccessor, IEmployeesRepository employeesRepository, IInvitesRepository invitesRepository, ITeamsRepository teamsRepository)
        {
            _contextAccessor = contextAccessor;
            _employeesRepository = employeesRepository;
            _invitesRepository = invitesRepository;
            _teamsRepository = teamsRepository;
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

        public async Task<IError> Register(string FirstName, string LastName, string PhoneNumber, string Position, DateTime BirthDay, string Password, string email, string SecretKey, int TeamId)
        {
            
            Invites invite = await _invitesRepository.GetByEmailAndSecretKey(email, SecretKey.Hash());
            if (invite == null)
                return new InviteNotFoundError();
            
            Teams team = await _teamsRepository.GetTeam(TeamId);
            if (team == null)
                return new TeamNotFoundError();


            Employees employee = new Employees
            {
                FirstName = FirstName,
                LastName = LastName,
                Position = Position,
                BirthDay = BirthDay,
                Email = email,
                PhoneNumber = PhoneNumber,
                Password = Password.Hash(),
                team = team,
                Manager = invite.AsManager

            };

            await _employeesRepository.CreateEmployee(employee);
            await  Login(employee);
            await _invitesRepository.deleteInvite(invite);

            return null;

        }
    }
}
