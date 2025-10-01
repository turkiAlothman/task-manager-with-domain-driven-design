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
using infrastructure.Services;
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
        private readonly IMinioService _minioService;
        
        public TasksService(IUnitOfWork unitOfWork, ITasksRepository tasksRepository, IEmployeesRepository employeesRepository, ICommentsRepository commentsRepository, IProjectsRepository projectsRepository, IConfiguration configuration, IMinioService minioService) : base(unitOfWork)
        {
            _tasksRepository = tasksRepository;
            _employeesRepository = employeesRepository;
            _commentsRepository = commentsRepository;
            _projectsRepository = projectsRepository;
            _configuration = configuration;
            _minioService = minioService;
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


            // after the task is commited in the database, upload attachments to MinIO
            if (!attachments.IsNullOrEmpty())
            {
                foreach ((Attachments attach, IFormFile file) in attachments.Zip(Attachments))
                {
                    try
                    {
                        // Create unique object name for MinIO: tasks/{taskId}/{filename}
                        string fileName = Path.GetFileName(file.FileName).Replace(" ", "_");
                        string objectName = $"tasks/{attach.task.Id}/{Guid.NewGuid()}_{fileName}";
                        
                        // Upload file to MinIO
                        string uploadedObjectName = await _minioService.UploadFileAsync(file, objectName);
                        
                        // Store MinIO object name and original filename
                        attach.url = uploadedObjectName;
                        attach.PhysicalPath = uploadedObjectName; // Using this field to store MinIO object name
                        attach.FileName = fileName; // Store the original filename
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to upload attachment: {ex.Message}");
                        // You might want to handle this error more gracefully
                        return new customError { Message = $"Failed to upload attachment: {file.FileName}", StatusCode = System.Net.HttpStatusCode.InternalServerError };
                    }
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

            // Delete attachments from MinIO before deleting the task
            if (task.Attachments != null && task.Attachments.Any())
            {
                foreach (var attachment in task.Attachments)
                {
                    try
                    {
                        // Delete file from MinIO using the object name stored in PhysicalPath
                        if (!string.IsNullOrEmpty(attachment.PhysicalPath))
                        {
                            await _minioService.DeleteFileAsync(attachment.PhysicalPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to delete attachment from MinIO: {ex.Message}");
                        // Continue with task deletion even if attachment deletion fails
                    }
                }
            }

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

        public async Task<string> GetAttachmentPresignedUrl(string objectName)
        {
            try
            {
                return await _minioService.GetPresignedUrlAsync(objectName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to generate presigned URL: {ex.Message}");
                return string.Empty;
            }
        }

    }
}
