using Application.Errors;
using Application.Errors.Employees;
using Application.Services.Interfaces;
using Domain.Base;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.Employee;
using Domain.Task;
using OneOf;
namespace infrastructure.Services
{
    public class EmployeesService : BaseService , IEmployeesService
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IEmployeesRepository _employeesRepository;

        public EmployeesService(
          IUnitOfWork unitOfWork, IProjectsRepository projectsRepository, IEmployeesRepository employeesRepository) : base(unitOfWork)
        {
            _projectsRepository = projectsRepository;
            _employeesRepository = employeesRepository;
        }

        public async Task<IEnumerable<Employees>> Search(string search = "", int? projectId = null)
        {
            return await _employeesRepository.GetAll(search, projectId: projectId);
        }
        public async Task<IEnumerable<Object>> ExeludeEmployees(int ProjectId, string? search = null)
        {
            return await _employeesRepository.ExeludeEmployees(ProjectId, search);
        }
        public async Task<OneOf<IEnumerable<Tasks>, IError>> GetEmployeeTasks(int id, bool Reported = false)
        {
            Employees employee = await _employeesRepository.GetEmployee(id);

            if (employee == null)
                return new EmployeeNotFoundError();


            IEnumerable<Tasks> tasks = await _employeesRepository.GetEmployeeTasks(employee, Reported);
            return OneOf<IEnumerable<Tasks>, IError>.FromT0(tasks);

        }
        public async Task<OneOf<IEnumerable<Object>, IError>> GetEmployeeActivities(int id)
        {
            Employees employee = await _employeesRepository.GetEmployee(id);

            if (employee == null)
                return new EmployeeNotFoundError();

            return OneOf<IEnumerable<Object>, IError>.FromT0(await _employeesRepository.GetEmployeeActivities(employee));
        }
    }
}
