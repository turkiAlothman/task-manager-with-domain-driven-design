using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Threading.Tasks;
using TaskManager.HttpExtensions;
using Domain.Models.DomainModels;
using Domain.Models.Repositories.interfaces;
using Domain.Models.Repositories.interfaces;
using TaskManager.RequestForms;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IProjectsRepository _projectsRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommentsRepository _commentsRepository;


        public TasksController(ITasksRepository tasksRepository, IEmployeesRepository _EmployeesRepository, IProjectsRepository _projectsRepository, IHttpContextAccessor _contextAccessor, ICommentsRepository _commentsRepository)
        {
            this._tasksRepository = tasksRepository;
            this._employeesRepository = _EmployeesRepository;
            this._projectsRepository = _projectsRepository;
            this._contextAccessor = _contextAccessor;
            this._commentsRepository = _commentsRepository;

        }

        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(_tasksRepository);
        }


        [HttpPost]
        public async Task<IActionResult> createTask([FromForm] AddTaskForm TaskData, IConfiguration _configuration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            List<Attachments> attachments = new List<Attachments>();
            foreach (IFormFile file in TaskData.Attachments)
            {
                (bool ValidationResult, string Message) = file.validate();

                if (!ValidationResult)
                    return BadRequest(new { Message = Message });

                attachments.Add(new Attachments());

            }


            await _tasksRepository.Add(new Tasks
            {
                Title = TaskData.title,
                Description = TaskData.description,
                Priority = TaskData.priority,
                Type = TaskData.type,
                Asignees = await _employeesRepository.getByIds(TaskData.asignees),
                StartDate = TaskData.startDate,
                DueDate = TaskData.deadline,
                Status = "Planned",
                Reporter = await _employeesRepository.GetEmployee(_contextAccessor.GetUserID()),
                Project = await _projectsRepository.GetById(TaskData.project),
                Attachments = attachments
            });

            if (!attachments.IsNullOrEmpty())
            {
                Directory.CreateDirectory((_configuration.GetValue<string>("TaskAttachmantsStoragePathUrl") + attachments.First().task.Id).Replace("static", _configuration.GetValue<string>("StoragePath")));
                foreach ((Attachments attach, IFormFile file) in attachments.Zip(TaskData.Attachments))
                {
                    string startPerfix = _configuration.GetValue<string>("TaskAttachmantsStoragePathUrl") + attach.task.Id;
                    string Url = startPerfix + "/" + Path.GetFileName(file.FileName).Replace(" ","_");
                    string PhysicalPath = Url.Replace("static", _configuration.GetValue<string>("StoragePath"));
                    string FullPath = Path.GetFullPath(PhysicalPath);
                    Debug.WriteLine(FullPath);

                    attach.url = Url;
                    attach.PhysicalPath = PhysicalPath;


                    using (var stream = System.IO.File.Create(FullPath))
                         await file.CopyToAsync(stream);
                }

            }






            return Ok();
        }

        [HttpDelete("{TaskID}")]
        public async Task<IActionResult> Delete(int TaskID)
        {
            Tasks task = await _tasksRepository.GetTaskWithAssigneesAndReporter(TaskID);

            if (task == null)
                return BadRequest(new {Message="Task does not exist"});
            if(!(task.Reporter.Id == _contextAccessor.GetUserID()))
                return Unauthorized(new { Message = "reporter is only the one who allowed to delete task" });

            await _tasksRepository.Delete(task);

            return Ok();

        }
        
        
        [HttpPatch("{TaskId}")]

        public async Task<IActionResult> update(int TaskId, [FromBody] JsonPatchDocument<Tasks> patch)
        {
            Tasks Origin = await this._tasksRepository.GetTask(TaskId);

            if (Origin == null)
            {
                return BadRequest();
            }

            if (! await _tasksRepository.IsAssigneeOrReporter(Origin.Id , _contextAccessor.GetUserID()))
            {
                return Unauthorized(new { Message = "only the Asignee or the reporter of this task is authorized to edit task" });
            }
            patch.ApplyTo(Origin, ModelState);

            if (!TryValidateModel(Origin))
                return BadRequest(ModelState);

            _tasksRepository.Update(Origin);

            return Ok(Origin);
        }

        [Route("addAsignee/{TaskId}/{asigneeID}")]
        [HttpPatch]
        public async Task<IActionResult> AddAsignee(int TaskId, int asigneeID)
        {

            Tasks task = await _tasksRepository.GetTaskWithAssigneesAndReporter(TaskId);
            
            if (task == null)
                return BadRequest("task not found");

            
            // the reporter is the only one allowed to assign employees
            if (task.Reporter.Id != _contextAccessor.GetUserID())
                return Unauthorized();

            Employees Employee = await this._employeesRepository.GetEmployee(asigneeID);

            if (Employee == null)
                return BadRequest("Employee not found");

            if (task.Asignees.Contains(Employee))
                return BadRequest(new Dictionary<string, string> { { "Message", "Employee already Asigned to this task" } });


            this._tasksRepository.AddAsignee(task, Employee);

            return Ok();
        }

        [Route("RemoveAsignee/{TaskId}/{asigneeID}")]
        [HttpPatch]
        public async Task<IActionResult> RemoveAsignee(int TaskId, int asigneeID)
        {
            Tasks task = await _tasksRepository.GetTaskWithAssigneesAndReporter(TaskId);
            if (task == null)
                return BadRequest("task not found");

            
            // the reporter is the only one allowed to remove employees
            if (task.Reporter.Id != _contextAccessor.GetUserID())
                return Unauthorized();

            Employees Employee = await this._employeesRepository.GetEmployee(asigneeID);

            if (Employee == null)
                return BadRequest("Employee not found");

            if (!task.Asignees.Contains(Employee))
                return BadRequest(new Dictionary<string, string> { { "Message", "Employee is not assigned to this task" } });

            this._tasksRepository.RemoveAsignee(task, Employee);

            return Ok();
        }



        [HttpPatch("AddComment/{TaskId}")]
        public async Task<IActionResult> AddComment(int TaskId, [FromBody] string MessageContent)
        {



            Tasks task = await _tasksRepository.GetTaskWithAssigneesAndReporter(TaskId);
            if (task == null)
                return NotFound(new { Message = "task not found" });

            Employees employee = await _employeesRepository.GetEmployee(_contextAccessor.GetUserID());

            if (employee == null)
                return BadRequest(new { Message = "Sender(Employee) not found" });

            this._tasksRepository.AddComment(task, new Comments
            {
                Sender = employee,
                MessageContent = MessageContent
            });

            return Ok();


        }

        [HttpDelete("DeleteComment/{CommentID}")]
        public async Task<IActionResult> DeleteComment(int CommentID)
        {
            Comments comment = await _commentsRepository.GetComment(CommentID);

            if (comment == null)
                return BadRequest(new { Message = "Comment not found" });

            if (comment.Sender.Id != _contextAccessor.GetUserID())
                return Unauthorized(new { Message = "only the sender of the comment can delete the message" });

            _commentsRepository.DeleteComment(comment);
            return Ok();


        }


    }
}
