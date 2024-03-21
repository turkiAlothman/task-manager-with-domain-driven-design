using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Runtime.Serialization;
using Domain.Models.DomainModels;
using Domain.Models.Repositories.interfaces;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IProjectsRepository _projectsRepository;

        public EmployeesController(IEmployeesRepository employeesRepository, IProjectsRepository projectsRepository)
        {
            _employeesRepository = employeesRepository;
            _projectsRepository = projectsRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> Search([FromQuery] string search="" , [FromQuery] int? projectId = null)
        {
            Projects? project = null;
            if (projectId!= null)
            {
                project = await _projectsRepository.GetById(projectId.Value);

            }
            
            return new JsonResult(await _employeesRepository.GetAll(search , project: project));
        }

        [HttpGet("Exelude/")]
        public async Task<IActionResult> ExeludeEmployees([FromQuery] int ProjectId ,[FromQuery] string? search = null )
        {
            return new JsonResult( await _employeesRepository.ExeludeEmployees(ProjectId , search));
        }
       
        
        
        
        [HttpGet("tasks/{id}")]
       public async Task<IActionResult> GetEmployeeTasks(int id, [FromQuery] bool Reported = false)
        {
            Employees employee = await _employeesRepository.GetEmployee(id);

            if (employee == null)
                return NotFound();

            return new JsonResult(await _employeesRepository.GetEmployeeTasks(employee, Reported));
        }




        [HttpGet("activities/{id}")]
        public async Task<IActionResult> GetEmployeeActivities(int id)
        {
            Employees employee = await _employeesRepository.GetEmployee(id);

            if (employee == null)
                return NotFound();

            return new JsonResult(await _employeesRepository.GetEmployeeActivities(employee));
        }



    }
}
