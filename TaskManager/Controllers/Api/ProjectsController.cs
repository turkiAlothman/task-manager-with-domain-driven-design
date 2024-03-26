using Application.Errors;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using OneOf.Types;
using TaskManager.HttpExtensions;
using TaskManager.RequestForms;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IProjectsService _projectsService;

        public ProjectsController(IHttpContextAccessor contextAccessor, IProjectsService projectsService)
        {
            _contextAccessor = contextAccessor;
            _projectsService = projectsService;
        }

        [HttpGet("employees/{projectsId}")]
        public async Task<IActionResult> GetProjectsEmployeesDetails(int projectsId)
        {
            var result = await _projectsService.GetProjectsEmployeesDetails(projectsId);
            return result.Match(
                 employees => Ok(employees),
                 error => StatusCode((int)error.StatusCode, error)
                 );
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectForm form)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IError result = await _projectsService.CreateProject(_contextAccessor.IsManager(), form.Name, form.Type, form.Description, form.StartDate, form.Deadline);
            if (result != null)
                return StatusCode((int)result.StatusCode, result);
            return Ok();
        }



        [HttpGet("tasks/{projectsId}")]
        public async Task<IActionResult> GetProjectsTasksDetails(int projectsId)
        {
            var result  = await _projectsService.GetProjectsTasksDetails(projectsId);

            return result.Match(
             tasks => Ok(tasks),
             error => StatusCode((int)error.StatusCode, error)
             );
        }

        [HttpGet("activities/{projectsId}")]
        public async Task<IActionResult> GetProjectsActivities(int projectsId)
        {
            var result = await _projectsService.GetProjectsActivities(projectsId);

            return result.Match(
             activities => Ok(activities),
             error => StatusCode((int)error.StatusCode, error)
             );
           
        }



        [HttpPost("{ProjectId}/employees/")]
        public async Task<IActionResult> AddEmployeeToProject([FromRoute] int ProjectId, [FromBody] List<int> EmployeesIds)
        {
            IError result = await _projectsService.AddEmployeeToProject(_contextAccessor.IsManager(), ProjectId, EmployeesIds);
            if(result != null)
                return StatusCode((int)result.StatusCode, result);

            return Ok();
        }


        [HttpDelete("{ProjectId}/employees/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int ProjectId, int employeeId)
        {
            IError result = await _projectsService.DeleteEmployee(_contextAccessor.IsManager(), ProjectId, employeeId);
            if (result != null)
                return StatusCode((int)result.StatusCode, result);

            return Ok();
        }
    }
}
