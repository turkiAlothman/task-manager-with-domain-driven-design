using Domain.Entities;
using Domain.Records;
namespace Domain.Models.Repositories.interfaces
{
    public interface IProjectsRepository
    {
        public  Task<IEnumerable<Projects>> GetAll();
        public Task<IEnumerable<Projects>> GetWithDetails();
        public Task<Projects> GetById(int id);
        public Task CreateProject(Projects project);
        public Task<IEnumerable<EmployeesDetailsWithinProjectResposne>?> GetProjectsEmployeesDetails(int  ProjectId);
        public Task<IEnumerable<ActivityRecord>> GetProjectsActivities(Projects project);
        public Task RemoveEmployee(Projects project , Employees employee);
        public Task AddListOfEmployeesInPoject(Projects project, List<Employees> employees);

    }
}
