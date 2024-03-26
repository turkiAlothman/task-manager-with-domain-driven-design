using System.ComponentModel.DataAnnotations;
using Domain.Employee;
using Domain.Entities;

namespace Domain.Team
{
    public partial class Teams : BaseEntity<int>
    {
        [Required(ErrorMessage = "team field is required")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Employees> Members { get; set; }
    }
}
