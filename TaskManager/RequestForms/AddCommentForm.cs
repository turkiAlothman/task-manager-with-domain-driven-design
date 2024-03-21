using System.ComponentModel.DataAnnotations;
using Domain.Models.DomainModels;

namespace TaskManager.RequestForms
{
    public class AddCommentForm
    {

        [Required]
        public string MessageContent { get; set; }

        [Required]
        public int SenderID { get; set; }
   
    }
}
