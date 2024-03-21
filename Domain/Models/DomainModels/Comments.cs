namespace Domain.Models.DomainModels
{
    public class Comments : BaseEntity
    {
        public string MessageContent { get; set;}

        public Employees Sender { get; set;}
        public Tasks MessageTask { get; set;}
    }
}
