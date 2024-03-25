using Domain.Base;
using Domain.Employee;

namespace Domain.Task
{
    public partial class Tasks : IAggregateRoot
    {
        //public Tasks(string? Title, DateTime StartDate, DateTime DueDate, string? Type, string? Priority, string? Description, string? Status)
        //{
        //    this.Title = Title;
        //    this.StartDate = StartDate;
        //    this.DueDate = DueDate;
        //    this.Type = Type;
        //    this.Priority = Priority;
        //    this.Description = Description;
        //    this.Status = Status;
        //}

        public void SetReporter(Employees employees)
        {

        }
    }
}
