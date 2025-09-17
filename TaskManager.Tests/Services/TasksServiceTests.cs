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
using infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TaskManager.Tests.Services
{
    public class TasksServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ITasksRepository> _mockTasksRepository;
        private readonly Mock<IEmployeesRepository> _mockEmployeesRepository;
        private readonly Mock<ICommentsRepository> _mockCommentsRepository;
        private readonly Mock<IProjectsRepository> _mockProjectsRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly TasksService _tasksService;

        public TasksServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTasksRepository = new Mock<ITasksRepository>();
            _mockEmployeesRepository = new Mock<IEmployeesRepository>();
            _mockCommentsRepository = new Mock<ICommentsRepository>();
            _mockProjectsRepository = new Mock<IProjectsRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _tasksService = new TasksService(_mockUnitOfWork.Object, _mockTasksRepository.Object, _mockEmployeesRepository.Object, _mockCommentsRepository.Object, _mockProjectsRepository.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnProjectNotFoundError_WhenProjectDoesNotExist()
        {
            // Arrange
            int userId = 1;
            string title = "Test Task";
            string description = "Test Description";
            var assignees = new List<int> { 2, 3 };
            int projectId = 999;
            string priority = "High";
            string type = "Feature";
            DateTime startDate = DateTime.Now;
            DateTime deadline = DateTime.Now.AddDays(7);
            var attachments = new List<IFormFile>();

            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync((Projects)null);

            // Act
            var result = await _tasksService.Create(userId, title, description, assignees, projectId, priority, type, startDate, deadline, attachments);

            // Assert
            Assert.IsType<ProjectNotFoundError>(result);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldCreateTask_WhenValidInputProvided()
        {
            // Arrange
            int userId = 1;
            string title = "Test Task";
            string description = "Test Description";
            var assignees = new List<int> { 2, 3 };
            int projectId = 1;
            string priority = "High";
            string type = "Feature";
            DateTime startDate = DateTime.Now;
            DateTime deadline = DateTime.Now.AddDays(7);
            var attachments = new List<IFormFile>();

            var project = new Projects("Test Project", "Web App", "Description", DateTime.Now, DateTime.Now.AddMonths(3));
            var reporter = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Manager", DateTime.Now);
            var assigneesList = new List<Employees>
            {
                new Employees(false, "Jane", "Smith", "987654321", "jane@test.com", "password", "Developer", DateTime.Now),
                new Employees(false, "Bob", "Johnson", "555666777", "bob@test.com", "password", "Designer", DateTime.Now)
            };

            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync(project);
            _mockEmployeesRepository.Setup(x => x.GetEmployee(userId))
                .ReturnsAsync(reporter);
            _mockEmployeesRepository.Setup(x => x.getByIds(assignees))
                .ReturnsAsync(assigneesList);
            _mockTasksRepository.Setup(x => x.Add(It.IsAny<Tasks>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _tasksService.Create(userId, title, description, assignees, projectId, priority, type, startDate, deadline, attachments);

            // Assert
            Assert.Null(result);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(userId), Times.Once);
            _mockEmployeesRepository.Verify(x => x.getByIds(assignees), Times.Once);
            _mockTasksRepository.Verify(x => x.Add(It.IsAny<Tasks>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnTaskNotFoundError_WhenTaskDoesNotExist()
        {
            // Arrange
            int userId = 1;
            int taskId = 999;
            var patch = new JsonPatchDocument<Tasks>();

            _mockTasksRepository.Setup(x => x.GetTask(taskId))
                .ReturnsAsync((Tasks)null);

            // Act
            var result = await _tasksService.Update(userId, taskId, patch);

            // Assert
            Assert.IsType<TaskNotFoundError>(result);
            _mockTasksRepository.Verify(x => x.GetTask(taskId), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnAuthorizationError_WhenUserNotAssigneeOrReporter()
        {
            // Arrange
            int userId = 1;
            int taskId = 1;
            var patch = new JsonPatchDocument<Tasks>();
            var task = new Tasks("Test Task", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature");

            _mockTasksRepository.Setup(x => x.GetTask(taskId))
                .ReturnsAsync(task);
            _mockTasksRepository.Setup(x => x.IsAssigneeOrReporter(taskId, userId))
                .ReturnsAsync(false);

            // Act
            var result = await _tasksService.Update(userId, taskId, patch);

            // Assert
            Assert.IsType<ReporterOrAssigneeAuthorizationError>(result);
            _mockTasksRepository.Verify(x => x.GetTask(taskId), Times.Once);
            // _mockTasksRepository.Verify(x => x.IsAssigneeOrReporter(taskId, userId), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnTaskNotFoundError_WhenTaskDoesNotExist()
        {
            // Arrange
            int userId = 1;
            int taskId = 999;

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync((Tasks)null);

            // Act
            var result = await _tasksService.Delete(userId, taskId);

            // Assert
            Assert.IsType<TaskNotFoundError>(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnAuthorizationError_WhenUserIsNotReporter()
        {
            // Arrange
            int userId = 1;
            int taskId = 1;
            var reporter = new Employees(false, "Jane", "Doe", "987654321", "jane@test.com", "password", "Manager", DateTime.Now);
            var task = new Tasks("Test Task", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature");
            task.SetReporter(reporter);

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync(task);

            // Act
            var result = await _tasksService.Delete(userId, taskId);

            // Assert
            Assert.IsType<UnathorizedTaskActionError>(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldDeleteTask_WhenUserIsReporter()
        {
            // Arrange
            int userId = 1;
            int taskId = 1;
            var reporter = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Manager", DateTime.Now);
            // Set the Id property manually since the constructor doesn't set it
            typeof(Employees).GetProperty("Id").SetValue(reporter, userId);
            
            var task = new Tasks("Test Task", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature");
            task.SetReporter(reporter);

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync(task);
            _mockTasksRepository.Setup(x => x.Delete(task))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _tasksService.Delete(userId, taskId);

            // Assert
            Assert.Null(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
            _mockTasksRepository.Verify(x => x.Delete(task), Times.Once);
        }

        [Fact]
        public async Task AddAsignee_ShouldReturnTaskNotFoundError_WhenTaskDoesNotExist()
        {
            // Arrange
            int userId = 1;
            int assigneeId = 2;
            int taskId = 999;

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync((Tasks)null);

            // Act
            var result = await _tasksService.AddAsignee(userId, assigneeId, taskId);

            // Assert
            Assert.IsType<TaskNotFoundError>(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
        }

        [Fact]
        public async Task AddAsignee_ShouldReturnAuthorizationError_WhenUserIsNotReporter()
        {
            // Arrange
            int userId = 1;
            int assigneeId = 2;
            int taskId = 1;
            var reporter = new Employees(false, "Jane", "Doe", "987654321", "jane@test.com", "password", "Manager", DateTime.Now);
            var task = new Tasks("Test Task", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature");
            task.SetReporter(reporter);

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync(task);

            // Act
            var result = await _tasksService.AddAsignee(userId, assigneeId, taskId);

            // Assert
            Assert.IsType<UnathorizedTaskActionError>(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
        }

        [Fact]
        public async Task AddAsignee_ShouldReturnEmployeeNotFoundError_WhenEmployeeDoesNotExist()
        {
            // Arrange
            int userId = 1;
            int assigneeId = 999;
            int taskId = 1;
            var reporter = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Manager", DateTime.Now);
            // Set the reporter Id to match userId
            typeof(Employees).GetProperty("Id").SetValue(reporter, userId);
            
            var task = new Tasks("Test Task", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature");
            task.SetReporter(reporter);

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync(task);
            _mockEmployeesRepository.Setup(x => x.GetEmployee(assigneeId))
                .ReturnsAsync((Employees)null);

            // Act
            var result = await _tasksService.AddAsignee(userId, assigneeId, taskId);

            // Assert
            Assert.IsType<EmployeeNotFoundError>(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(assigneeId), Times.Once);
        }

        [Fact]
        public async Task AddAsignee_ShouldReturnAlreadyAssignedError_WhenEmployeeAlreadyAssigned()
        {
            // Arrange
            int userId = 1;
            int assigneeId = 2;
            int taskId = 1;
            var reporter = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Manager", DateTime.Now);
            // Set the reporter Id to match userId
            typeof(Employees).GetProperty("Id").SetValue(reporter, userId);
            
            var assignee = new Employees(false, "Jane", "Smith", "987654321", "jane@test.com", "password", "Developer", DateTime.Now);
            typeof(Employees).GetProperty("Id").SetValue(assignee, assigneeId);
            
            var task = new Tasks("Test Task", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature");
            task.SetReporter(reporter);
            task.AddAssignees(new List<Employees> { assignee });

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync(task);
            _mockEmployeesRepository.Setup(x => x.GetEmployee(assigneeId))
                .ReturnsAsync(assignee);

            // Act
            var result = await _tasksService.AddAsignee(userId, assigneeId, taskId);

            // Assert
            Assert.IsType<AlreadyAssignedError>(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(assigneeId), Times.Once);
        }

        [Fact]
        public async Task RemoveAsignee_ShouldReturnNotAssignedError_WhenEmployeeNotAssigned()
        {
            // Arrange
            int userId = 1;
            int assigneeId = 2;
            int taskId = 1;
            var reporter = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Manager", DateTime.Now);
            // Set the reporter Id to match userId
            typeof(Employees).GetProperty("Id").SetValue(reporter, userId);
            
            var assignee = new Employees(false, "Jane", "Smith", "987654321", "jane@test.com", "password", "Developer", DateTime.Now);
            typeof(Employees).GetProperty("Id").SetValue(assignee, assigneeId);
            
            var task = new Tasks("Test Task", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature");
            task.SetReporter(reporter);

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync(task);
            _mockEmployeesRepository.Setup(x => x.GetEmployee(assigneeId))
                .ReturnsAsync(assignee);

            // Act
            var result = await _tasksService.RemoveAsignee(userId, assigneeId, taskId);

            // Assert
            Assert.IsType<NotAssignedError>(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(assigneeId), Times.Once);
        }

        [Fact]
        public async Task AddComment_ShouldReturnTaskNotFoundError_WhenTaskDoesNotExist()
        {
            // Arrange
            int userId = 1;
            int taskId = 999;
            string messageContent = "Test comment";

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync((Tasks)null);

            // Act
            var result = await _tasksService.AddComment(userId, taskId, messageContent);

            // Assert
            Assert.IsType<TaskNotFoundError>(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
        }

        [Fact]
        public async Task AddComment_ShouldAddComment_WhenValidRequest()
        {
            // Arrange
            int userId = 1;
            int taskId = 1;
            string messageContent = "Test comment";
            var reporter = new Employees(false, "Jane", "Doe", "987654321", "jane@test.com", "password", "Manager", DateTime.Now);
            var commenter = new Employees(false, "John", "Smith", "123456789", "john@test.com", "password", "Developer", DateTime.Now);
            var task = new Tasks("Test Task", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature");
            task.SetReporter(reporter);

            _mockTasksRepository.Setup(x => x.GetTaskWithAssigneesAndReporter(taskId))
                .ReturnsAsync(task);
            _mockEmployeesRepository.Setup(x => x.GetEmployee(userId))
                .ReturnsAsync(commenter);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _tasksService.AddComment(userId, taskId, messageContent);

            // Assert
            Assert.Null(result);
            _mockTasksRepository.Verify(x => x.GetTaskWithAssigneesAndReporter(taskId), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(userId), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnCommentNotFoundError_WhenCommentDoesNotExist()
        {
            // Arrange
            int userId = 1;
            int commentId = 999;

            _mockCommentsRepository.Setup(x => x.GetComment(commentId))
                .ReturnsAsync((Comments)null);

            // Act
            var result = await _tasksService.DeleteComment(userId, commentId);

            // Assert
            Assert.IsType<CommentNotFoundError>(result);
            _mockCommentsRepository.Verify(x => x.GetComment(commentId), Times.Once);
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnAuthorizationError_WhenUserIsNotCommentSender()
        {
            // Arrange
            int userId = 1;
            int commentId = 1;
            var sender = new Employees(false, "Jane", "Doe", "987654321", "jane@test.com", "password", "Developer", DateTime.Now);
            var comment = new Comments("Test comment");
            comment.SetSender(sender);

            _mockCommentsRepository.Setup(x => x.GetComment(commentId))
                .ReturnsAsync(comment);

            // Act
            var result = await _tasksService.DeleteComment(userId, commentId);

            // Assert
            Assert.IsType<UnathorizedCommentDeleteError>(result);
            _mockCommentsRepository.Verify(x => x.GetComment(commentId), Times.Once);
        }

        [Fact]
        public async Task DeleteComment_ShouldDeleteComment_WhenUserIsCommentSender()
        {
            // Arrange
            int userId = 1;
            int commentId = 1;
            var sender = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Developer", DateTime.Now);
            // Set the sender Id to match userId
            typeof(Employees).GetProperty("Id").SetValue(sender, userId);
            
            var comment = new Comments("Test comment");
            comment.SetSender(sender);

            _mockCommentsRepository.Setup(x => x.GetComment(commentId))
                .ReturnsAsync(comment);
            _mockCommentsRepository.Setup(x => x.DeleteComment(comment));

            // Act
            var result = await _tasksService.DeleteComment(userId, commentId);

            // Assert
            Assert.Null(result);
            _mockCommentsRepository.Verify(x => x.GetComment(commentId), Times.Once);
            _mockCommentsRepository.Verify(x => x.DeleteComment(comment), Times.Once);
        }
    }
}
