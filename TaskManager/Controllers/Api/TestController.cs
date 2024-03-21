using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Security.Claims;
using infrastructure.Extentions;
using infrastructure.Persistence;
using Domain.Models.DomainModels;
using Domain.Models.Repositories.interfaces;
using Application.Services.Interfaces;

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
        
        public TestController(IHttpContextAccessor contextAccessor, IProjectsRepository _projectsRepository , IEmployeesRepository employeesRepository, TaskManagerDbContext _dbContext, IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            this._projectsRepository = _projectsRepository;
            this._employeesRepository = employeesRepository;
            this._dbContext = _dbContext;

        }
        [HttpGet]
        public async Task<IActionResult> Index(IConfiguration _configuration , IEmailService mail )
        {
            
            //await mail.SendMail(new Models.Models.Mail { EmailTo = "toto2014a.a@gmail.com", Subject = "test" });

            return Ok(new {message = _contextAccessor.HttpContext.User.FindFirstValue("manager") });
        }
    }
}
