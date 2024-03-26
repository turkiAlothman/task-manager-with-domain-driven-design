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

        public static Employees CreateDummy(dynamic data)
        {
            Type  type = data.GetType();
            
            Employees employee = new Employees(
                type.GetProperty("Manager").GetValue(data, null),
                type.GetProperty("FirstName").GetValue(data, null),
                type.GetProperty("LastName").GetValue(data, null) ,
                type.GetProperty("PhoneNumber").GetValue(data, null),
                type.GetProperty("Email").GetValue(data, null),
                type.GetProperty("Password").GetValue(data, null),
                type.GetProperty("Position").GetValue(data, null),
                type.GetProperty("BirthDay").GetValue(data, null));
            employee.SetTeam(type.GetProperty("team").GetValue(data, null));
            employee.AddProjects(type.GetProperty("Projects").GetValue(data, null)); 
            return employee; 
        }
        public static Object dd(dynamic data)
        {
            Type type = data.GetType();

            return type.GetProperty("Projects").GetValue(data, null);

            Employees employee = new Employees(data.Manager, data.FirstName, data.LastName, data.PhoneNumber, data.Email, data.Password, data.Position, data.BirthDay);
            employee.SetTeam(data.team);
            employee.AddProjects(data.Projects);
            return employee;
        }

    }
}
