using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.HttpExtensions;
using Domain.Models.Repositories.interfaces;
using RandomString4Net;
using infrastructure.Extentions;
using Domain.Models;
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
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IInviteService _inviteService;
        public InviteController( IHttpContextAccessor contextAccessor, IInviteService inviteService)
        {
            _contextAccessor = contextAccessor;
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
