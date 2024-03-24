using Application.Services.Interfaces;
using Domain.Models.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet()]
        public async Task<IActionResult> Search([FromQuery] string search = "", [FromQuery] int? projectId = null)
        {
            return new JsonResult(await _employeesService.Search(search, projectId));
        }

        [HttpGet("Exelude/")]
        public async Task<IActionResult> ExeludeEmployees([FromQuery] int ProjectId, [FromQuery] string? search = null)
        {
            return new JsonResult(await _employeesService.ExeludeEmployees(ProjectId, search));
        }




        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetEmployeeTasks(int id, [FromQuery] bool Reported = false)
        {
            var result = await _employeesService.GetEmployeeTasks(id, Reported);
            return result.Match(
                tasks => Ok(tasks),
                error => StatusCode((int)error.StatusCode, error)
            );

        }




        [HttpGet("activities/{id}")]
        public async Task<IActionResult> GetEmployeeActivities(int id)
        {
            var result = await _employeesService.GetEmployeeActivities(id);
            return result.Match(
                 activities => Ok(activities),
                 error => StatusCode((int)error.StatusCode, error)
                 );
        }



    }
}
