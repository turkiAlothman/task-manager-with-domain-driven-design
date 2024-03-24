using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models.Repositories.interfaces;
using infrastructure.Extentions;
using Application.Errors.Authentication;
using Application.Errors.Authorizations;
using Application.Errors;

namespace infrastructure.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IResetPasswordRepository _resetPasswordRepository;
        public ResetPasswordService(IResetPasswordRepository resetPasswordRepository) {
            _resetPasswordRepository = resetPasswordRepository;
        }
        public async Task<IError> CheckResetPasswordRequest(string email, string secretKey)
        {
            ResetPassword resetPassword = await _resetPasswordRepository.GetByEmailAndSecret(email, secretKey.Hash());

            if (resetPassword == null || resetPassword.ExpirationDate < DateTime.Now)
                return new NotAuthorizedResetPasswordRequest();
            return null;
        }
    }
}
