using Application.Errors;
using Domain.Employee;
namespace TaskManager.Services
{
    public interface IAuthService
    {
        public Task<IError> LogInWithCheck(string email , string password, bool StayLoggedIn);
        public Task Login(Employees employee, bool StayLoggedIn = true);
        public Task<IError> Register(string FirstName, string LastName, string PhoneNumber, string Position, DateTime BirthDay, string Password, string email, string SecretKey, int TeamId);
        public Task<IError> ResetPasswordRequest(string Email,string host);
        public Task<IError> ResetPassword(string Password, string Email , string SecretKey);


    }
}
