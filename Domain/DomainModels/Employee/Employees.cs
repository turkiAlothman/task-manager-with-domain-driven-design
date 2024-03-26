using System.ComponentModel;
using Domain.Activitiy;
using Domain.Entities;
using Domain.Project;
using Domain.Team;
using Domain.Task;

namespace Domain.Employee
{
    public partial class Employees : BaseEntity<int>
    {
        public bool Manager { get; protected set; }

        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string Position { get; protected set; }
        public DateTime BirthDay { get; protected set;}
        public IList<Projects> Projects { get; protected set; } = new List<Projects>();
        public IList<Tasks> Tasks { get; protected set; } = new List<Tasks>();
        public IList<Tasks> tasksReported { get; protected set; } = new List<Tasks>();
        public Teams team { get; protected set; } = null;

        public IList<Activities> activities { get; protected set; } = new List<Activities>();
    }
}
