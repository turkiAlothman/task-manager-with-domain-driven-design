namespace Domain.Entities
{
    public class Invites : BaseEntity<int>
    {
        public Employees inviter { get; set; }
        public string inviteeEmail { get; set; }
        public bool AsManager { get; set; } = false;
        public string SecretKey { get; set; }
    }
}
