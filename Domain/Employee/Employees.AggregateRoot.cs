using Domain.Base;
using Domain.Project;
using Domain.Team;

namespace Domain.Employee
{
    public partial class Employees : IAggregateRoot
    {
        public Employees(
        bool Manager,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email,
        string Password,
        string Position,
        DateTime BirthDay)
    {
        this.Manager = Manager;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.PhoneNumber = PhoneNumber;
        this.Email = Email;
        this.Password = Password;
        this.Position = Position;
        this.BirthDay = BirthDay;
    }

        public void ChangePassword(string newPassword)
        {
            this.Password = newPassword;
        }
        public void SetTeam(Teams team)
        {
            this.team = team;
        }
        public void AddProject(Projects Project)
        {
            this.Projects.Add(Project);
        }
        public void AddProjects(List<Projects> Projects)
        {
            ((List<Projects>)this.Projects).AddRange(Projects);
        }

        public static Employees CreateDummy(dynamic fake)
        {
            Employees employee = new Employees(fake.Manager, fake.FirstName, fake.LastName, fake.PhoneNumber, fake.Email, fake.Password, fake.Position, fake.BirthDay);
            employee.SetTeam(fake.team);
            employee.AddProjects(fake.Projects); 
            return employee; 
        }

}
}
