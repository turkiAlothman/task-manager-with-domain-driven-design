using Domain.Models.DomainModels;
using Domain.Models.Models;

namespace Application.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendMail(Mail mail);
        public Task SendInviteEmail(Employees inviter, string inviteeEmail, string SecretKey, string host);
        public Task SendResetPasswordEmail(string Email, string SecretKey, string host);
    }
}
