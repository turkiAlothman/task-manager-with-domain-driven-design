using Domain.Employee;
using Domain.Entities;
using Domain.Task;

namespace Domain.Project
{
    public partial class Projects : BaseEntity<int>
    {
        public string Name { protected set; get; }
        public string Type { protected set; get; }
        public string Description { protected set; get; }
        public DateTime StartDate { protected set; get; }
        public DateTime DueDate { protected set; get; }
        public IList<Tasks> Tasks { protected set; get; } = new List<Tasks>();
        public IList<Employees> Employees { protected set; get; } = new List<Employees>();
    }
}

