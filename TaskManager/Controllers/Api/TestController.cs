using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Security.Claims;
using infrastructure.Extentions;
using infrastructure.Persistence;
using Domain.Entities;
using Application.Services.Interfaces;
using Application.Errors;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.DomainModels.Team;
using Domain.Employee;
using Domain.Project;
using Domain.Team;
using Domain.DomainModels.Task;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Options;
using infrastructure.Configurations;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IProjectsRepository _projectsRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly TaskManagerDbContext _dbContext;
        private readonly ITeamsRepository _teamsRepository;
        private readonly ITasksRepository _tasksRepository;
        private readonly IConfiguration _configuration;
        private readonly IOptions<MailSettings> MailSettings;


        public TestController(IHttpContextAccessor contextAccessor, IProjectsRepository _projectsRepository , IEmployeesRepository employeesRepository, TaskManagerDbContext _dbContext, IConfiguration configuration, ITeamsRepository teamsRepository,ITasksRepository tasksRepository, IOptions<MailSettings> MailSettings) 
        {
            _contextAccessor = contextAccessor;
            this._projectsRepository = _projectsRepository;
            this._employeesRepository = employeesRepository;
            this._dbContext = _dbContext;
            this._teamsRepository = teamsRepository;
            this._tasksRepository = tasksRepository;
            this._configuration = configuration;
            this.MailSettings = MailSettings;

        }
        [HttpGet]
        public async Task<IActionResult> Index(IConfiguration _configuration, IEmailService mail)
        {

            //throw new TaskNotFoundException();
            int i = 0;
            //this.MailSettings       
            return Ok(new {mail = Environment.GetEnvironmentVariable("MAIL_USERNAME") , data = this.MailSettings });
        }
    }
}
