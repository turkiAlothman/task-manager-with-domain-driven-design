using System.ComponentModel.DataAnnotations;

namespace TaskManager.RequestForms
{
    public class CreateProjectForm
    {
        [Required]
        [MaxLength(50)]
        public string Name { set; get; }

        [Required]
        [MaxLength(40)]
        public string Type { set; get; }

        [Required]
        [MaxLength(500)]
        public string Description { set; get; }

        [Required]
        public DateTime StartDate { set; get; }
        
        [Required]
        public DateTime DueDate { set; get; }
    }
}
