using System.ComponentModel.DataAnnotations;
using TaskManager.Validators;

namespace TaskManager.RequestForms
{
    public class ResetPasswordForm
    {
        [Required]
        [MinLength(8)]
        [MaxLength(40)]
        [Password]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(40)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string SecretKey { get; set; }
    }
}
