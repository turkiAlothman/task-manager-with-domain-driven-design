using Domain.Employee;
using Domain.Entities;
using Domain.Task;

namespace Domain.Activitiy
{
    public partial class Activities : BaseEntity<int>
    {
        public Tasks? task { get; set; }
        public Employees actor { get; set; }
        public string description { get; set; }
        public string ProjectName { get; set; }
    }
}
