using Domain.Employee;
using Domain.Entities;

namespace Domain.DomainModels.Employee
{
    public class Invites : BaseEntity<int>
    {
        public Employees inviter { get; set; }
        public string inviteeEmail { get; set; }
        public bool AsManager { get; set; } = false;
        public string SecretKey { get; set; }
    }
}
