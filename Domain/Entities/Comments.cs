namespace Domain.Entities
{
    public class Comments : BaseEntity<int>
    {
        public string MessageContent { get; set; }

        public Employees Sender { get; set; }
        public Tasks MessageTask { get; set; }
    }
}
