using Domain.Base;
using Domain.Employee;
using Domain.Task;

namespace Domain.Project
{
    public partial class Projects : IAggregateRoot
    {
        public Projects(string Name, string Type, string Description, DateTime StartDate, DateTime DueDate)
        {
            this.Name = Name;
            this.Type = Type;
            this.Description = Description;
            this.StartDate = StartDate;
            this.DueDate = DueDate;
        }
        public Projects() { }

        public void AddTask(Tasks task)
        {
            this.Tasks.Add(task);
        }
        public void AddTasks(List<Tasks> tasks)
        {
            ((List<Tasks>)this.Tasks).AddRange(tasks);
        }
        public void AddEmployee(Employees employee)
        {
            this.Employees.Add(employee);
        }
        public void AddEmployees(List<Employees> employees)
        {
            ((List<Employees>)this.Employees).AddRange(employees);
        }
    }
}
