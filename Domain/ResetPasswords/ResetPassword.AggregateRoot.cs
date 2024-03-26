using Domain.Base;

namespace Domain.ResetPasswords
{
    public partial class ResetPassword : IAggregateRoot
    {
        public ResetPassword(string Email, string? SecretKey, DateTime ExpirationDate)
        {
            this.Email = Email;
            this.SecretKey = SecretKey;
            this.ExpirationDate = ExpirationDate;
        }
    }
}
