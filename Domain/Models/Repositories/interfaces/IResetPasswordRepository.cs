using Domain.Models.DomainModels;

namespace Domain.Models.Repositories.interfaces
{
    public interface IResetPasswordRepository
    {
        public Task CreateResetPasswordRequest(ResetPassword resetPassword);
        public Task<ResetPassword> GetByEmail(string email);
        public Task<ResetPassword> GetByEmailAndSecret(string email,string SecretKey);

    }
}
