using Domain.Employee;
using Domain.Entities;
using Domain.Task;

namespace Domain.Comment
{
    public partial class Comments : BaseEntity<int>
    {
        public string MessageContent { get; set; }

        public Employees Sender { get; set; }
        public Tasks MessageTask { get; set; }
    }
}
