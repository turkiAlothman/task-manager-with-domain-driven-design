using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Domain.Activitiy;
using Domain.Comment;
using Domain.Employee;
using Domain.Entities;
using Domain.Project;

namespace Domain.Task
{
    public partial class Tasks : BaseEntity<int>
    {
        [StringLength(50)]
        public string? Title { get; private set; } = string.Empty;
        public DateTime StartDate { get; protected set; }
        public DateTime DueDate { get;  protected set; }

        [StringLength(50)]
        public string? Type { get;  protected set; } = string.Empty;


        [AllowedValues("High", "Medium", "Low")]

        public string? Priority { get; protected set; } = string.Empty;

        [StringLength(400)]
        public string? Description { get; protected set; } = string.Empty;


        [AllowedValues("Canceled", "Planned", "In Progress", "Done")]
        public string? Status { get; protected set; } = string.Empty;

        public Projects? Project { get; protected set; } = null;


        public IList<Employees>? Asignees { get; protected set; } = new List<Employees>();


        public Employees? Reporter { get; protected set; } = null;

        public IList<Comments>? Comments { get; protected set; } = new List<Comments>();
        public IList<Activities>? Activities { get; protected set; } = new List<Activities>();
        public IList<Attachments>? Attachments { get; protected set; } = new List<Attachments>();



    }
}
