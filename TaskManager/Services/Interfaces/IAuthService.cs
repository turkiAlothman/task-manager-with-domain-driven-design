using TaskManager.Models.DomainModels;

namespace TaskManager.Services.Interfaces
{
    public interface IAuthService
    {
        public Task Login(Employees employee, bool StayLoggedIn = true);
    }
}
