

using Application.Errors;
using Domain.Employee;

namespace Application.Services.Interfaces
{
    public interface IInviteService
    {
        public Task<IError> InviteEmployee(bool IsManager, int UserId, string Host, String email, bool AsManager);
        public Task<Invites> GetInvite(string email, string SecretKey);
    
    }
}
