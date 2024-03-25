using Domain.Entities;

namespace Domain.ResetPasswords
{
    public partial class  ResetPassword : BaseEntity<int>
    {
        public string Email { get; set; }
        public string? SecretKey { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
