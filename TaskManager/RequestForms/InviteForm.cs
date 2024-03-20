using System.ComponentModel.DataAnnotations;

namespace TaskManager.RequestForms
{
    public class InviteForm
    {
        [EmailAddress]
        [Required]
        public String email {  get; set; }
        public bool AsManager { get; set; } = false;
    }
}
