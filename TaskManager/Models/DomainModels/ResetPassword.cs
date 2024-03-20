namespace TaskManager.Models.DomainModels
{
    public class ResetPassword : BaseEntity
    {
        public string Email { get; set; }
        public string? SecretKey { get; set;}
        public DateTime ExpirationDate { get; set;}
    }
}
