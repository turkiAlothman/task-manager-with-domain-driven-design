using Domain.ResetPasswords;

namespace Domain.DomainModels.ResetPasswords
{
    public interface IResetPasswordRepository
    {
        public System.Threading.Tasks.Task CreateResetPasswordRequest(ResetPassword resetPassword);
        public Task<ResetPassword> GetByEmail(string email);
        public Task<ResetPassword> GetByEmailAndSecret(string email, string SecretKey);

    }
}
