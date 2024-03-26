using Application.Services.Interfaces;
using infrastructure.Extentions;
using Application.Errors.Authentication;
using Application.Errors.Authorizations;
using Application.Errors;
using Domain.Base;
using Domain.ResetPasswords;
using Domain.DomainModels.ResetPasswords;

namespace infrastructure.Services
{
    public class ResetPasswordService :BaseService ,  IResetPasswordService
    {
        private readonly IResetPasswordRepository _resetPasswordRepository;
        public ResetPasswordService(IUnitOfWork unitOfWork, IResetPasswordRepository resetPasswordRepository) :base(unitOfWork)
        {
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
