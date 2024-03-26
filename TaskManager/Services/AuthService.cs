using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Services;
using Application.Errors;
using Domain.Models.Repositories.interfaces;
using Application.Errors.Authentication;
using infrastructure.Persistence.Repositores;
using TaskManager.RequestForms;
using Application.Services.Interfaces;
using infrastructure.Extentions;
using Application.Errors.Invites;
using Application.Errors.Teams;
using RandomString4Net;
using Application.Errors.ResetPassword;
using Application.Errors.Employees;
using Application.Errors.Authorizations;
using Domain.Employee;
using Domain.Team;
using Domain.ResetPasswords;

namespace TaskManager.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IInvitesRepository _invitesRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly IResetPasswordRepository _resetPasswordRepository;
        private readonly IEmailService _emailService;
        public AuthService(IHttpContextAccessor contextAccessor, IEmployeesRepository employeesRepository, IInvitesRepository invitesRepository, ITeamsRepository teamsRepository, IResetPasswordRepository resetPasswordRepository, IEmailService emailService)
        {
            _contextAccessor = contextAccessor;
            _employeesRepository = employeesRepository;
            _invitesRepository = invitesRepository;
            _teamsRepository = teamsRepository;
            _resetPasswordRepository = resetPasswordRepository;
            _emailService = emailService;
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
            (
                invite.AsManager,
                FirstName,
                LastName,
                PhoneNumber,
                email,
                Password.Hash(),
                Position,
                BirthDay
            );
            employee.SetTeam(team);

            await _employeesRepository.CreateEmployee(employee);
            await  Login(employee);
            await _invitesRepository.deleteInvite(invite);

            return null;

        }

        public async Task<IError> ResetPasswordRequest(string Email , string host)
        {
            ResetPassword ResetPasswordRequest = await _resetPasswordRepository.GetByEmail(Email);

            if (ResetPasswordRequest != null && ResetPasswordRequest.ExpirationDate > DateTime.Now)
            {
                return new ResetPasswordAlreadyRequestedError();
            }

            Employees employee = await _employeesRepository.GetByEmail(Email);

            if (employee == null)
                return new EmailNotFoundError();


            // request expire after 20 minutes
            DateTime ExpirationDate = DateTime.Now.AddMinutes(20);
            string SecretKey = RandomString.GetString(Types.ALPHABET_LOWERCASE, 30);


            // send email 
            await this._emailService.SendResetPasswordEmail(Email, SecretKey,host);

            await _resetPasswordRepository.CreateResetPasswordRequest(new ResetPassword { Email = Email, ExpirationDate = ExpirationDate, SecretKey = SecretKey.Hash() });
            return null;

        }

        public async Task<IError> ResetPassword(string Password, string Email, string SecretKey)
        {
            ResetPassword ResetPasswordRequest = await _resetPasswordRepository.GetByEmailAndSecret(Email, SecretKey.Hash());

            if (ResetPasswordRequest == null)
                return new NotAuthorizedResetPasswordRequest();

            if (ResetPasswordRequest.ExpirationDate < DateTime.Now)
                return new ResetPasswordRequestExpiredError();

            Employees employee = await _employeesRepository.GetByEmail(Email);
            if (employee == null)
                return new EmployeeNotFoundError();

            employee.ChangePassword(Password.Hash());

            await _employeesRepository.Update(employee);
            
            return null;
        }
    }
}
