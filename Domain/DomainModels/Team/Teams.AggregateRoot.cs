
using Domain.Base;

namespace Domain.Team
{
    public partial class Teams : IAggregateRoot
    {
        public Teams(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
        }
        public Teams() { }
    }
}
