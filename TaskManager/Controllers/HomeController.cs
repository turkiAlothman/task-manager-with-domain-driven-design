using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using TaskManager.HttpExtensions;
using TaskManager.RequestForms;
using TaskManager.Utilities;
using Domain.Task;
using Domain.Activitiy;
using Domain.DomainModels.Activitiy;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.DomainModels.Task;
using Domain.DomainModels.Team;
using Domain.DTOs;
namespace TaskManager.Controllers
{

    public class HomeController : Controller
    {
        private readonly ITasksRepository _TasksRepository;
        private readonly IEmployeesRepository _EmployeesRepository;
        private readonly IProjectsRepository _ProjectsRepository;
        private readonly IActivitiesRepository _ActivitiesRepository;
        private readonly ITeamsRepository _TeamsRepository;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public HomeController(ITasksRepository _tasksRepository, IEmployeesRepository employeesRepository, IProjectsRepository _ProjectsRepository, IActivitiesRepository activitiesRepository, ITeamsRepository teamsRepository, IHttpContextAccessor HttpContextAccessor)
        {
            this._TasksRepository = _tasksRepository;
            this._EmployeesRepository = employeesRepository;
            this._ProjectsRepository = _ProjectsRepository;
            this._ActivitiesRepository = activitiesRepository;
            this._TeamsRepository = teamsRepository;
            this._HttpContextAccessor = HttpContextAccessor;
        }
        public async Task<IActionResult> Dashboard()
        {
           int ActivitiesCount = await this._ActivitiesRepository.count();
           int ProjectsCount = await _ProjectsRepository.Count();
           int TasksCount = await this._TasksRepository.Count();
           int TeamsCount = await this._TeamsRepository.Count();

            Dashboard data = new Dashboard {
                ActivitiesCount = ActivitiesCount,
                ProjectsCount = ProjectsCount, 
                TasksCount = TasksCount,
                TeamsCount = TeamsCount 
            };
            return View(data);
        }
        public async Task<IActionResult> Tasks(TasksFilterQuery query)
        {

            (IEnumerable<Tasks> tasks, int count) = await _TasksRepository.GetAll(query.pageIndex , query.pageSize, query.ProjectId , query.TeamId , query.AssignedToMe, query.search , query.Status, query.Priority, _HttpContextAccessor.GetUserID());



            return View(new PageList<Tasks>(tasks.ToList(), count, query.pageIndex, query.pageSize));
        }
        public async Task<IActionResult> Projects()
        {
            return View(await _ProjectsRepository.GetAll());
        }

        public async Task<IActionResult> Employees([FromQuery] string Search = "", [FromQuery] int? TeamId = null)
        {
            return View(await _EmployeesRepository.GetAll(Search, TeamId));
        }
        public async Task<IActionResult> Teams()
        {
            return View(await _TeamsRepository.GetAllWithDetails());
        }
        public async Task<IActionResult> Activities(int pageIndex = 1, int pageSize = 60)
        {
            IEnumerable<Activities> activities = await this._ActivitiesRepository.GetAllActivities(pageIndex, pageSize);
            int count = await _ActivitiesRepository.count();

            return View(new PageList<Activities>(activities.ToList(), count, pageIndex, pageSize));
        }



        [HttpGet("/[controller]/[action]/{id}")]
        public async Task<IActionResult> TaskDetails(int id)
        {
            return View(await _TasksRepository.GetTaskWithAllDetails(id));
        }

        public async Task<IActionResult> Profile()
        {
            return View(await _EmployeesRepository.GetProfile(_HttpContextAccessor.GetUserID()));
        }
    }
}
