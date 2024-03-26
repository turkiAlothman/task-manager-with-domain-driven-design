using Domain.Entities;

namespace Domain.ResetPasswords
{
    public partial class  ResetPassword : BaseEntity<int>
    {
        public string Email { get; protected set; }
        public string? SecretKey { get; protected set; }
        public DateTime ExpirationDate { get; protected set; }
    }
}
