using Domain.Base;
using Domain.Employee;
using Domain.Task;

namespace Domain.Activitiy
{
    public partial class Activities : IAggregateRoot
    {
        public Activities(string description, string ProjectName)
        {
            this.description = description;
            this.ProjectName = ProjectName;
        }


        public void SetActor(Employees actor)
        {
            this.actor = actor;
        }

        public void SetTask(Tasks task)
        {
            this.task = task;
        }
    }
}
