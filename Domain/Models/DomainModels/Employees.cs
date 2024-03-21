using System.ComponentModel;

namespace Domain.Models.DomainModels
{
    public class Employees : BaseEntity
    {
        public bool Manager { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Position { get; set; }
        public DateTime BirthDay {  get; set; }
        public IList<Projects> Projects { get; set; }
        public IList<Tasks> Tasks { get; set; }
        public IList<Tasks> tasksReported { get; set; }
        public Teams team { get; set; }

        public IList<Activities> activities { get; set; }
    }
}
