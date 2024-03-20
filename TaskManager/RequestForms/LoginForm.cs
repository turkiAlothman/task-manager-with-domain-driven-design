using System.ComponentModel.DataAnnotations;

namespace TaskManager.RequestForms
{
    public class LoginForm
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(50)]

        public string Password { get; set; }

        
        public bool StayLoggedIn { get; set; } = false;

        public string? returnUrl { get; set; } = "/";
    }
}
