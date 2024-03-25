using Application.Errors;
using Application.Errors.Authorizations;
using Application.Errors.Employees;
using Application.Errors.Tasks;
using Application.Services.Interfaces;
using Domain.Base;
using Domain.Models.Repositories.interfaces;
using Domain.DTOs;
using infrastructure.Persistence.Repositores;
using OneOf;
using Domain.Task;
using Domain.Employee;
using Domain.Project;

namespace infrastructure.Services
{
    public class ProjectsService : BaseService,IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly ITasksRepository _tasksRepository;
        private readonly IEmployeesRepository _employeesRepository;
        public ProjectsService(IUnitOfWork unitOfWork , IProjectsRepository projectsRepository, ITasksRepository tasksRepository, IEmployeesRepository employeesRepository) :base(unitOfWork)
        {
            _projectsRepository = projectsRepository;
            _tasksRepository = tasksRepository;
            _employeesRepository = employeesRepository;
        }
        public async Task<OneOf<IEnumerable<EmployeesDetailsWithinProjectResposne>, IError>> GetProjectsEmployeesDetails(int projectsId)
        {
            IEnumerable<EmployeesDetailsWithinProjectResposne> ProjectEmployees = await _projectsRepository.GetProjectsEmployeesDetails(projectsId);
            if (ProjectEmployees == null)
                return new ProjectNotFoundError();

            return OneOf<IEnumerable<EmployeesDetailsWithinProjectResposne>, IError>.FromT0(ProjectEmployees);
        }


        public async Task<IError> CreateProject(bool IsManager, string Name, string Type, string Description, DateTime StartDate, DateTime DueDate)
        {
            if (!IsManager)
                return new ManagerAuthorizationError();

            await _projectsRepository.CreateProject(new Projects
            {
                Name = Name,
                Description = Description,
                Type = Type,
                StartDate = StartDate,
                DueDate = DueDate,

            });

            return null;
        }


        public async Task<OneOf<IEnumerable<Tasks>, IError>> GetProjectsTasksDetails(int ProjectId)
        {
            Projects project = await _projectsRepository.GetById(ProjectId);

            if (project == null)
                return new ProjectNotFoundError();

            return OneOf<IEnumerable<Tasks>, IError>.FromT0(await _tasksRepository.GetByProject(project));
        }
        public async Task<OneOf<IEnumerable<ActivityRecord>, IError>> GetProjectsActivities(int ProjectId)
        {
            Projects project = await _projectsRepository.GetById(ProjectId);

            if (project == null)
                return new ProjectNotFoundError();

            return OneOf<IEnumerable<ActivityRecord>, IError>.FromT0(await _projectsRepository.GetProjectsActivities(project));
        }


        public async Task<IError> AddEmployeeToProject(bool IsManager, int ProjectId, List<int> EmployeesIds)
        {
            if (!IsManager)
                return new ManagerAuthorizationError();

            Projects project = await _projectsRepository.GetById(ProjectId);

            if (project == null)
                return new ProjectNotFoundError();

            List<Employees> employees = await _employeesRepository.GetEmployeesByListOfIds(EmployeesIds, project);

            await _projectsRepository.AddListOfEmployeesInPoject(project, employees);
            return null;

        }

        public async Task<IError> DeleteEmployee(bool IsManager, int ProjectId, int employeeId)
        {
            if (!IsManager)
                return new UnathorizedTaskActionError();

            Projects project = await _projectsRepository.GetById(ProjectId);
            if (project == null)
                return new ProjectNotFoundError();

            Employees employee = await _employeesRepository.GetEmployee(employeeId);
            if (employee == null)
                return new EmployeeNotFoundError();

            await _projectsRepository.RemoveEmployee(project, employee);
            return null;
        }
    }
}
