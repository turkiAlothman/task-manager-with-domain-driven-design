using Domain.Employee;
using Domain.Entities;
using Domain.Task;

namespace Domain.Comment
{
    public partial class Comments : BaseEntity<int>
    {
        public string MessageContent { get; protected set; }

        public Employees Sender { get; protected set; }
        public Tasks MessageTask { get; protected set; }
    }
}
