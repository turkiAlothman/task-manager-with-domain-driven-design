using Application.Errors;
using Domain.Entities;
using OneOf;
namespace TaskManager.Services
{
    public interface IAuthService
    {
        public Task<IError> LogInWithCheck(string email , string password, bool StayLoggedIn);
        public Task Login(Employees employee, bool StayLoggedIn = true);
    }
}
