using System.ComponentModel.DataAnnotations;

namespace TaskManager.RequestForms
{

    public class AddTaskForm
    {
        [Required]
        [StringLength(50)]
        public string title { get; set; }
        
        [Required]

        [StringLength(400)]
        public string description { get; set; }

        public List<int> asignees{ get; set; } = new List<int>();

        [Required]
        public int project {  get; set; }

        [Required]
        [AllowedValues("High", "Medium", "Low")]
        public string priority { get; set; }

        [Required]
        public string type { get; set; }


        [Required]
        public DateTime startDate { get; set; }
        
        [Required]
        public DateTime deadline { get; set; }

        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();



    }
}
