using Domain.Entities;

namespace Domain.Models.Repositories.interfaces
{
    public interface IProjectsRepository
    {
        public  Task<IEnumerable<Projects>> GetAll();
        public Task<IEnumerable<Projects>> GetWithDetails();
        public Task<Projects> GetById(int id);
        public Task CreateProject(Projects project);
        public Task<Object?> GetProjectsEmployeesDetails(Projects project);
        public Task<IEnumerable<Object>> GetProjectsActivities(Projects project);
        public Task RemoveEmployee(Projects project , Employees employee);
        public Task AddListOfEmployeesInPoject(Projects project, List<Employees> employees);

    }
}
