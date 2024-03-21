using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManager.HttpExtensions;
using Domain.Models.DomainModels;
using infrastructure.Persistence.Repositores;

using Domain.Models.Repositories.interfaces;
using TaskManager.RequestForms;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly ITasksRepository _tasksRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProjectsController(IProjectsRepository projectsRepository, ITasksRepository tasksRepository, IEmployeesRepository employeesRepository, IHttpContextAccessor contextAccessor)
        {
            _projectsRepository = projectsRepository;
            _tasksRepository = tasksRepository;
            _employeesRepository = employeesRepository;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("employees/{projectsId}")]
        public async Task<IActionResult> GetProjectsEmployeesDetails(int projectsId)
        {
            Projects project = await _projectsRepository.GetById(projectsId);

            if(project == null)
                return NotFound();

            return Ok(await _projectsRepository.GetProjectsEmployeesDetails(project));
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectForm form)
        {
            if (!_contextAccessor.IsManager())
                return Unauthorized(new { Message = "Manager is only allowed to create new Project" });

            await _projectsRepository.CreateProject(new Projects
            {
                Name = form.Name,
                Description = form.Description,
                Type = form.Type,
                StartDate = form.StartDate,
                DueDate = form.DueDate,
                
            });

            return Ok();
        }



        [HttpGet("tasks/{projectsId}")]
        public async Task<IActionResult> GetProjectsTasksDetails(int projectsId)
        {
            Projects project = await _projectsRepository.GetById(projectsId);

            if (project == null)
                return NotFound();

            return Ok(await _tasksRepository.GetByProject(project));
        }

        [HttpGet("activities/{projectsId}")]
        public async Task<IActionResult> GetProjectsActivities(int projectsId)
        {
            Projects project = await _projectsRepository.GetById(projectsId);

            if (project == null)
                return NotFound();

            return Ok(await _projectsRepository.GetProjectsActivities(project));
        }


        [HttpDelete("{ProjectId}/employees/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int ProjectId, int employeeId )
        {
            if (!_contextAccessor.IsManager())
                return Unauthorized(new { Message = "Manager is only allowed to remove Employee To Project" });

            Projects project = await _projectsRepository.GetById(ProjectId);
            if (project == null)
                return NotFound(new { Message="Project not Found"});
            
            Employees employee = await _employeesRepository.GetEmployee(employeeId);
            if(employee == null)
                return NotFound(new { Message = "Employee not Found" });


            await _projectsRepository.RemoveEmployee(project, employee);
            return  Ok();
        }

        [HttpPost("{ProjectId}/employees/")]
        public async Task<IActionResult> AddEmployeeToProject([FromRoute] int ProjectId , [FromBody] List<int> EmployeesIds)
        {
            if(!_contextAccessor.IsManager())
                return Unauthorized(new {Message ="Manager is only allowed to add Employee To Project"});

           Projects project = await _projectsRepository.GetById(ProjectId);

           if (project == null)
               return NotFound(new { Message="Project Not Found"});

           List<Employees> employees = await _employeesRepository.GetEmployeesByListOfIds(EmployeesIds,project);

            await _projectsRepository.AddListOfEmployeesInPoject(project, employees);


            return Ok();
        }
    }
}
