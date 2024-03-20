using TaskManager.Models.DomainModels;
using TaskManager.Models.Models;

namespace TaskManager.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendMail(Mail mail);
        public Task SendInviteEmail(Employees inviter, string inviteeEmail, string SecretKey);
        public Task SendResetPasswordEmail(string Email, string SecretKey);
    }
}
