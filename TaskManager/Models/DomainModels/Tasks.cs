//using static System.Runtime.InteropServices.JavaScript.JSType;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TaskManager.Models.DomainModels
{
    public class Tasks : BaseEntity
    {
        [StringLength(50)]
        public string? Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } 
        public DateTime DueDate { get; set; }

        [StringLength(50)]
        public string? Type { get; set; } = string.Empty;


        [AllowedValues("High", "Medium", "Low")]

        public string? Priority { get; set; } = string.Empty;
        
        [StringLength(400)]
        public string? Description { get; set; } = string.Empty;

        
        [AllowedValues("Canceled", "Planned", "In Progress", "Done")]
        public string? Status { get; set; } = string.Empty;

        [BindNever]
        public Projects? Project { get; set; }

        [BindNever]
        public IList<Employees>? Asignees { get; set; }

        
        public Employees? Reporter { get; set; } = null;

        public IList<Comments>? Comments {get;set;}
        public IList<Activities>? Activities { get; set; }
        public IList<Attachments>? Attachments { get; set; }
        


    }
}
