using Domain.Entities;
using Domain.Task;

namespace Domain.DomainModels.Task
{
    public class Attachments : BaseEntity<int>
    {
        public string? url { get; set; }
        public string? PhysicalPath { get; set; }
        public string? FileName { get; set; }
        public Tasks task { get; set; }
    }
}
