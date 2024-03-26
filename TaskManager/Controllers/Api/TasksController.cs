using Application.Errors;
using Application.Services.Interfaces;
using Domain.Task;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.HttpExtensions;
using TaskManager.RequestForms;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITasksService _tasksService;


        public TasksController(IHttpContextAccessor _contextAccessor, ITasksService tasksService)
        {
            this._contextAccessor = _contextAccessor;
            this._tasksService = tasksService;

        }

        [HttpPost]
        public async Task<IActionResult> createTask([FromForm] AddTaskForm TaskData, IConfiguration _configuration)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            IError result = await _tasksService.Create(_contextAccessor.GetUserID(), TaskData.title, TaskData.description, TaskData.asignees, TaskData.project, TaskData.priority, TaskData.type, TaskData.startDate, TaskData.deadline, TaskData.Attachments);
            if (result != null) return StatusCode((int)result.StatusCode , result);
            return Ok();
        }

        [HttpDelete("{TaskID}")]
        public async Task<IActionResult> Delete(int TaskID)
        {
            IError result = await _tasksService.Delete(_contextAccessor.GetUserID(), TaskID);

            if (result != null)
            {
                return StatusCode((int)result.StatusCode, result);
            }
            return Ok();

        }


        [HttpPatch("{TaskId}")]

        public async Task<IActionResult> update(int TaskId, [FromBody] JsonPatchDocument<Tasks> patch)
        {
            IError result = await _tasksService.Update(_contextAccessor.GetUserID(), TaskId, patch);
            if (result != null) return StatusCode((int)result.StatusCode, result);
            return Ok();
        }

        [Route("addAsignee/{TaskId}/{asigneeID}")]
        [HttpPatch]
        public async Task<IActionResult> AddAsignee(int TaskId, int asigneeID)
        {
            IError result = await this._tasksService.AddAsignee(_contextAccessor.GetUserID(), asigneeID, TaskId);
            if (result != null)
                return StatusCode((int)result.StatusCode, result);
            return Ok();
        }

        [Route("RemoveAsignee/{TaskId}/{asigneeID}")]
        [HttpPatch]
        public async Task<IActionResult> RemoveAsignee(int TaskId, int asigneeID)
        {
            IError result = await this._tasksService.RemoveAsignee(_contextAccessor.GetUserID(), asigneeID, TaskId);
            if (result != null) return StatusCode((int)result.StatusCode, result);
            return Ok();
        }



        [HttpPatch("AddComment/{TaskId}")]
        public async Task<IActionResult> AddComment(int TaskId, [FromBody] string MessageContent)
        {
            IError result = await _tasksService.AddComment(_contextAccessor.GetUserID(), TaskId, MessageContent);
            if (result != null) return StatusCode((int)result.StatusCode, result);
            return Ok();
        }

        [HttpDelete("DeleteComment/{CommentID}")]
        public async Task<IActionResult> DeleteComment(int CommentID)
        {
            IError result = await this._tasksService.DeleteComment(_contextAccessor.GetUserID(), CommentID);
            if (result != null) return StatusCode((int)result.StatusCode, result);
            return Ok();
        }


    }
}
