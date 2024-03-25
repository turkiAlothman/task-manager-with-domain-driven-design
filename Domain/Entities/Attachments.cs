namespace Domain.Entities
{
    public class Attachments : BaseEntity<int>
    {
        public string? url { get; set; }
        public string? PhysicalPath { get; set; }
        public Tasks task { get; set; }
    }
}
