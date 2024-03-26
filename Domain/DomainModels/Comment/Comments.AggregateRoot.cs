using Domain.Base;
using Domain.Employee;
using Domain.Task;

namespace Domain.Comment
{
    public partial class Comments : IAggregateRoot
    {
        public Comments(string MessageContent)
        {
            this.MessageContent = MessageContent;
        }

        public void SetSender(Employees sender)
        {
            this.Sender = sender;
        }
        public void SetTask(Tasks task)
        {
            this.MessageTask = task;
        }
    }
}
