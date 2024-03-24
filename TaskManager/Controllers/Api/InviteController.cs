using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.HttpExtensions;
using Domain.Models.Repositories.interfaces;
using RandomString4Net;
using infrastructure.Extentions;
using Domain.Models.Models;
using TaskManager.RequestForms;
using System.Security.Claims;
using Application.Services.Interfaces;
using Domain.Entities;
using Application.Errors;
namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly IInvitesRepository _invitesRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailService _emailService;
        private readonly IInviteService _inviteService;
        public InviteController(IInvitesRepository invitesRepository, IEmployeesRepository employeesRepository, IHttpContextAccessor contextAccessor, IEmailService emailService, IInviteService inviteService)
        {
            _invitesRepository = invitesRepository;
            _employeesRepository = employeesRepository;
            _contextAccessor = contextAccessor;
            _emailService = emailService;
            _inviteService = inviteService;
        }

        [HttpPost]
        public async Task<IActionResult> InviteEmployee([FromBody] InviteForm form)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            IError result = await _inviteService.InviteEmployee(_contextAccessor.IsManager(), _contextAccessor.GetUserID(), _contextAccessor.HttpContext.Request.Host.Value, form.email, form.AsManager);

            if (result != null) { return StatusCode((int)result.StatusCode, result); }

            return Ok(new {Message="email invited successfully!"});


        }
    }
}
