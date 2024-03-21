using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DomainModels
{
    public class Teams : BaseEntity
    {
        [Required(ErrorMessage ="team field is required")]
        public int Id { get; set;}
        public string Name { get; set;}
        public string Description { get; set;}
        public IList<Employees> Members { get; set;}
    }
}
