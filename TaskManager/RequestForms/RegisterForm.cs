
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TaskManager.Extentions;
using TaskManager.ExtentionsMethods;
using TaskManager.Validators;
namespace TaskManager.RequestForms
{
    public class RegisterForm
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        
        [Required]
        public string Position { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

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
        public string email {  get; set; }  
        
        
        [Required]
        public string SecretKey { get; set; }

        [Required(ErrorMessage ="team field is required")]
        
        
        public int TeamId { get; set; }

}
}
