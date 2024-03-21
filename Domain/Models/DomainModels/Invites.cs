namespace Domain.Models.DomainModels
{
    public class Invites : BaseEntity
    {
        public Employees inviter { get; set; }
        public string inviteeEmail { get; set; }
        public bool AsManager { get; set; } = false;
        public string SecretKey { get; set;}
    }
}
