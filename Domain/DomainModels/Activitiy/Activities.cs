using Domain.Employee;
using Domain.Entities;
using Domain.Task;

namespace Domain.Activitiy
{
    public partial class Activities : BaseEntity<int>
    {
        public Tasks? task { get; protected set; }
        public Employees actor { get; protected set; }
        public string description { get; protected set; }
        public string ProjectName { get; protected set; }
    }
}
