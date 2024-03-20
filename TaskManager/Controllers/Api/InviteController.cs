using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Extentions;
using TaskManager.Models.DomainModels;
using TaskManager.Models.Repositories.implementers;
using TaskManager.Models.Repositories.interfaces;
using RandomString4Net;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using TaskManager.ExtentionsMethods;
using TaskManager.Services.Implementers;
using TaskManager.Services.Interfaces;
using TaskManager.Models.Models;
using TaskManager.RequestForms;
using System.Security.Claims;

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
        public InviteController(IInvitesRepository invitesRepository, IEmployeesRepository employeesRepository, IHttpContextAccessor contextAccessor, IEmailService emailService)
        {
            _invitesRepository = invitesRepository;
            _employeesRepository = employeesRepository;
            _contextAccessor = contextAccessor;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> InviteEmployee([FromBody] InviteForm form)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(! _contextAccessor.IsManager() )
                return Unauthorized(new { Message ="only manager who authorized to invite an employee"});

            Employees employee = await _employeesRepository.GetByEmail(form.email);
            
            // check if Employee already exist
            if(employee != null) 
                return BadRequest(new {Message = "this email already exists"});

            Invites invite  = await _invitesRepository.GetByEmail(form.email);
            
            // check if the requested email already invited
            if (invite != null)
                return BadRequest(new { Message = "this email already invited" });
            
            
            Employees inviter = await _employeesRepository.GetEmployee(_contextAccessor.GetUserID());
            
            string SecretKey =  RandomString.GetString(Types.ALPHABET_LOWERCASE, 10);

            // send SecretKey to email with view 
            _emailService.SendInviteEmail(inviter, form.email, SecretKey);


            await _invitesRepository.CreateInvite(new Invites { inviteeEmail = form.email, AsManager = form.AsManager,inviter = inviter , SecretKey = SecretKey.Hash() });
                    

            return Ok(new {Message="email invited successfully!"});


        }
    }
}
