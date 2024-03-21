using Domain.Entities;

namespace TaskManager.Services
{
    public interface IAuthService
    {
        public Task Login(Employees employee, bool StayLoggedIn = true);
    }
}
