using Application.Errors;
using Application.Errors.Comments;
using Application.Errors.Employees;
using Application.Errors.Tasks;
using Application.Services.Interfaces;
using Domain.Base;
using Domain.Comment;
using Domain.DomainModels.Comment;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.DomainModels.Task;
using Domain.Employee;
using Domain.Project;
using Domain.Task;
using infrastructure.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace infrastructure.Services
{
    public class TasksService : BaseService, ITasksService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IProjectsRepository _projectsRepository;
        private readonly IConfiguration _configuration;
        public TasksService(IUnitOfWork unitOfWork, ITasksRepository tasksRepository, IEmployeesRepository employeesRepository, ICommentsRepository commentsRepository, IProjectsRepository projectsRepository, IConfiguration configuration) : base(unitOfWork)
        {
            _tasksRepository = tasksRepository;
            _employeesRepository = employeesRepository;
            _commentsRepository = commentsRepository;
            _projectsRepository = projectsRepository;
            _configuration = configuration;
        }



        public async Task<IError> Create(int UserId, string title, string description, List<int> asignees, int project, string priority, string type, DateTime startDate, DateTime deadline, List<IFormFile> Attachments)
        {
            List<Attachments> attachments = new List<Attachments>();
            foreach (IFormFile file in Attachments)
            {
                (bool ValidationResult, string Message) = file.validate();

                if (!ValidationResult)
                    return new customError { Message = Message, StatusCode = System.Net.HttpStatusCode.BadRequest };

                attachments.Add(new Attachments());
            }


            Projects Project = await _projectsRepository.GetById(project);

            if (Project == null)
            {
                return new ProjectNotFoundError();
            }

            Tasks task = new Tasks(title, startDate, deadline, priority, description, "Planned", type);
            task.SetReporter(await _employeesRepository.GetEmployee(UserId));
            task.AddAssignees(await _employeesRepository.getByIds(asignees));
            task.SetProject(Project);
            task.AddAttachments(attachments);
            await _tasksRepository.Add(task);


            // after the task is commited in the database , we get the Id of the task to specify the storage path of the attachments
            if (!attachments.IsNullOrEmpty())
            {
                Directory.CreateDirectory((_configuration.GetValue<string>("TaskAttachmantsStoragePathUrl") + attachments.First().task.Id).Replace("static", _configuration.GetValue<string>("StoragePath")));
                foreach ((Attachments attach, IFormFile file) in attachments.Zip(Attachments))
                {
                    string startPerfix = _configuration.GetValue<string>("TaskAttachmantsStoragePathUrl") + attach.task.Id;
                    string Url = startPerfix + "/" + Path.GetFileName(file.FileName).Replace(" ", "_");
                    string PhysicalPath = Url.Replace("static", _configuration.GetValue<string>("StoragePath"));
                    string FullPath = Path.GetFullPath(PhysicalPath);
                    Debug.WriteLine(FullPath);

                    attach.url = Url;
                    attach.PhysicalPath = PhysicalPath;


                    using (var stream = System.IO.File.Create(FullPath))
                        await file.CopyToAsync(stream);
                }

            }

            await UnitOfWork.SaveChangesAsync();


            return null;



        }
        public async Task<IError> Update(int UserId, int TaskID, JsonPatchDocument<Tasks> patch)
        {
            Tasks Origin = await this._tasksRepository.GetTask(TaskID);

            if (Origin == null)
            {
                return new TaskNotFoundError();
            }

            if (!await _tasksRepository.IsAssigneeOrReporter(Origin.Id, UserId))
            {
                return new ReporterOrAssigneeAuthorizationError();
            }

            var result = new List<ValidationResult>();
            var context = new ValidationContext(Origin, null, null);

            patch.ApplyToTask(Origin);



            if (!Validator.TryValidateObject(Origin, context, result, true))
                return new customError { Message = result.First().ErrorMessage.ToString(), StatusCode = System.Net.HttpStatusCode.BadRequest };

            await UnitOfWork.SaveChangesAsync();

            return null;
        }
        public async Task<IError> Delete(int UserId, int TaskID)
        {
            Tasks task = await _tasksRepository.GetTaskWithAssigneesAndReporter(TaskID);

            if (task == null)
                return new TaskNotFoundError();
            if (!(task.Reporter.Id == UserId))
                return new UnathorizedTaskActionError();

            await _tasksRepository.Delete(task);
            return null;

        }

        public async Task<IError> AddAsignee(int UserId, int AssigneeId, int TaskId)
        {
            Tasks task = await _tasksRepository.GetTaskWithAssigneesAndReporter(TaskId);

            if (task == null)
                return new TaskNotFoundError();


            // the reporter is the only one allowed to assign employees
            if (task.Reporter.Id != UserId)
                return new UnathorizedTaskActionError();

            Employees Employee = await this._employeesRepository.GetEmployee(AssigneeId);

            if (Employee == null)
                return new EmployeeNotFoundError();

            if (task.Asignees.Contains(Employee))
                return new AlreadyAssignedError();

            this._tasksRepository.AddAsignee(task, Employee);
            return null;
        }

        public async Task<IError> RemoveAsignee(int UserId, int AssigneeId, int TaskId)
        {
            Tasks task = await _tasksRepository.GetTaskWithAssigneesAndReporter(TaskId);
            if (task == null)
                return new TaskNotFoundError();


            // the reporter is the only one allowed to remove employees
            if (task.Reporter.Id != UserId)
                return new UnathorizedTaskActionError();

            Employees Employee = await this._employeesRepository.GetEmployee(AssigneeId);

            if (Employee == null)
                return new EmployeeNotFoundError();

            if (!task.Asignees.Contains(Employee))
                return new NotAssignedError();

            this._tasksRepository.RemoveAsignee(task, Employee);

            return null;
        }

        public async Task<IError> AddComment(int UserId, int TaskId, string MessageContent)
        {
            Tasks task = await _tasksRepository.GetTaskWithAssigneesAndReporter(TaskId);
            if (task == null)
                return new TaskNotFoundError();

            Employees employee = await _employeesRepository.GetEmployee(UserId);

            Comments comment = new Comments(MessageContent);
            comment.SetSender(employee);
            task.AddComment(comment);

            await UnitOfWork.SaveChangesAsync();

            return null;
        }

        public async Task<IError> DeleteComment(int UserId, int CommentID)
        {
            Comments comment = await _commentsRepository.GetComment(CommentID);

            if (comment == null)
                return new CommentNotFoundError();

            if (comment.Sender.Id != UserId)
                return new UnathorizedCommentDeleteError();

            _commentsRepository.DeleteComment(comment);
            return null;
        }

    }
}