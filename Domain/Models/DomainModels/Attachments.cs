namespace Domain.Models.DomainModels
{
    public class Attachments: BaseEntity
    {
        public string? url { get; set; }
        public string? PhysicalPath { get; set; }
        public Tasks task { get; set; }
    }
}
