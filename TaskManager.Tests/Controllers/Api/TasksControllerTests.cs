using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TaskManager.Controllers.Api;
using TaskManager.RequestForms;
using Application.Errors;
using Domain.Task;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Tests.Controllers.Api
{
    public class TasksControllerTests : BaseControllerTest
    {
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _controller = new TasksController(MockHttpContextAccessor.Object, MockTasksService.Object);
            SetupControllerContext(_controller);
        }

        [Fact]
        public async Task CreateTask_WithValidForm_ReturnsOkResult()
        {
            // Arrange
            var taskForm = new AddTaskForm
            {
                title = "New Task",
                description = "Task Description",
                asignees = new List<int> { 1, 2 },
                project = 1,
                priority = "High",
                type = "Development",
                startDate = DateTime.Now,
                deadline = DateTime.Now.AddDays(7),
                Attachments = null
            };

            MockTasksService.Setup(s => s.Create(
                It.IsAny<string>(), // userId
                taskForm.title,
                taskForm.description,
                taskForm.asignees,
                taskForm.project,
                taskForm.priority,
                taskForm.type,
                taskForm.startDate,
                taskForm.deadline,
                taskForm.Attachments))
                .ReturnsAsync((IError?)null);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.createTask(taskForm, MockConfiguration.Object);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateTask_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var taskForm = new AddTaskForm
            {
                title = "", // Invalid - empty title
                description = "Description"
            };

            _controller.ModelState.AddModelError("title", "Title is required");

            // Act
            var result = await _controller.createTask(taskForm, MockConfiguration.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task CreateTask_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            var taskForm = new AddTaskForm
            {
                title = "Test Task",
                description = "Description",
                project = 1
            };

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.Unauthorized);

            MockTasksService.Setup(s => s.Create(
                It.IsAny<string>(),
                taskForm.title,
                taskForm.description,
                taskForm.asignees,
                taskForm.project,
                taskForm.priority,
                taskForm.type,
                taskForm.startDate,
                taskForm.deadline,
                taskForm.Attachments))
                .ReturnsAsync(error.Object);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.createTask(taskForm, MockConfiguration.Object);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(401, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Delete_WithValidTaskId_ReturnsOkResult()
        {
            // Arrange
            const int taskId = 1;

            MockTasksService.Setup(s => s.Delete(
                It.IsAny<string>(), // userId
                taskId))
                .ReturnsAsync((IError?)null);

            // Act
            var result = await _controller.Delete(taskId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int taskId = 999;

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.NotFound);

            MockTasksService.Setup(s => s.Delete(
                It.IsAny<string>(),
                taskId))
                .ReturnsAsync(error.Object);

            // Act
            var result = await _controller.Delete(taskId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Update_WithValidPatch_ReturnsOkResult()
        {
            // Arrange
            const int taskId = 1;
            var patchDoc = new JsonPatchDocument<Tasks>();
            patchDoc.Replace(t => t.Title, "Updated Title");

            MockTasksService.Setup(s => s.Update(
                It.IsAny<string>(), // userId
                taskId,
                patchDoc))
                .ReturnsAsync((IError?)null);

            // Act
            var result = await _controller.update(taskId, patchDoc);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Update_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int taskId = 999;
            var patchDoc = new JsonPatchDocument<Tasks>();
            patchDoc.Replace(t => t.Title, "Updated Title");

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.Forbidden);

            MockTasksService.Setup(s => s.Update(
                It.IsAny<string>(),
                taskId,
                patchDoc))
                .ReturnsAsync(error.Object);

            // Act
            var result = await _controller.update(taskId, patchDoc);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddAsignee_WithValidData_ReturnsOkResult()
        {
            // Arrange
            const int taskId = 1;
            const int assigneeId = 2;

            MockTasksService.Setup(s => s.AddAsignee(
                It.IsAny<string>(), // userId
                assigneeId,
                taskId))
                .ReturnsAsync((IError?)null);

            // Act
            var result = await _controller.AddAsignee(taskId, assigneeId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddAsignee_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int taskId = 1;
            const int assigneeId = 999;

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.NotFound);

            MockTasksService.Setup(s => s.AddAsignee(
                It.IsAny<string>(),
                assigneeId,
                taskId))
                .ReturnsAsync(error.Object);

            // Act
            var result = await _controller.AddAsignee(taskId, assigneeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task RemoveAsignee_WithValidData_ReturnsOkResult()
        {
            // Arrange
            const int taskId = 1;
            const int assigneeId = 2;

            MockTasksService.Setup(s => s.RemoveAsignee(
                It.IsAny<string>(), // userId
                assigneeId,
                taskId))
                .ReturnsAsync((IError?)null);

            // Act
            var result = await _controller.RemoveAsignee(taskId, assigneeId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemoveAsignee_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int taskId = 1;
            const int assigneeId = 999;

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.BadRequest);

            MockTasksService.Setup(s => s.RemoveAsignee(
                It.IsAny<string>(),
                assigneeId,
                taskId))
                .ReturnsAsync(error.Object);

            // Act
            var result = await _controller.RemoveAsignee(taskId, assigneeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddComment_WithValidComment_ReturnsOkResult()
        {
            // Arrange
            const int taskId = 1;
            const string messageContent = "This is a test comment";

            MockTasksService.Setup(s => s.AddComment(
                It.IsAny<string>(), // userId
                taskId,
                messageContent))
                .ReturnsAsync((IError?)null);

            // Act
            var result = await _controller.AddComment(taskId, messageContent);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddComment_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int taskId = 999;
            const string messageContent = "Test comment";

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.NotFound);

            MockTasksService.Setup(s => s.AddComment(
                It.IsAny<string>(),
                taskId,
                messageContent))
                .ReturnsAsync(error.Object);

            // Act
            var result = await _controller.AddComment(taskId, messageContent);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task DeleteComment_WithValidCommentId_ReturnsOkResult()
        {
            // Arrange
            const int commentId = 1;

            MockTasksService.Setup(s => s.DeleteComment(
                It.IsAny<string>(), // userId
                commentId))
                .ReturnsAsync((IError?)null);

            // Act
            var result = await _controller.DeleteComment(commentId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteComment_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int commentId = 999;

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.Unauthorized);

            MockTasksService.Setup(s => s.DeleteComment(
                It.IsAny<string>(),
                commentId))
                .ReturnsAsync(error.Object);

            // Act
            var result = await _controller.DeleteComment(commentId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(401, statusCodeResult.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task Delete_WithDifferentTaskIds_CallsServiceCorrectly(int taskId)
        {
            // Arrange
            MockTasksService.Setup(s => s.Delete(
                It.IsAny<string>(),
                taskId))
                .ReturnsAsync((IError?)null);

            // Act
            await _controller.Delete(taskId);

            // Assert
            MockTasksService.Verify(s => s.Delete(
                It.IsAny<string>(),
                taskId), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Short comment")]
        [InlineData("This is a very long comment that contains multiple sentences and should still be handled correctly by the system.")]
        public async Task AddComment_WithDifferentCommentLengths_CallsServiceCorrectly(string messageContent)
        {
            // Arrange
            const int taskId = 1;

            MockTasksService.Setup(s => s.AddComment(
                It.IsAny<string>(),
                taskId,
                messageContent))
                .ReturnsAsync((IError?)null);

            // Act
            await _controller.AddComment(taskId, messageContent);

            // Assert
            MockTasksService.Verify(s => s.AddComment(
                It.IsAny<string>(),
                taskId,
                messageContent), Times.Once);
        }

        [Fact]
        public async Task CreateTask_WithMultipleAssignees_CallsServiceCorrectly()
        {
            // Arrange
            var taskForm = new AddTaskForm
            {
                title = "Multi-Assignee Task",
                description = "Description",
                asignees = new List<int> { 1, 2, 3, 4 },
                project = 1
            };

            MockTasksService.Setup(s => s.Create(
                It.IsAny<string>(),
                taskForm.title,
                taskForm.description,
                taskForm.asignees,
                taskForm.project,
                taskForm.priority,
                taskForm.type,
                taskForm.startDate,
                taskForm.deadline,
                taskForm.Attachments))
                .ReturnsAsync((IError?)null);

            _controller.ModelState.Clear();

            // Act
            await _controller.createTask(taskForm, MockConfiguration.Object);

            // Assert
            MockTasksService.Verify(s => s.Create(
                It.IsAny<string>(),
                taskForm.title,
                taskForm.description,
                It.Is<List<int>>(list => list.Count == 4),
                taskForm.project,
                taskForm.priority,
                taskForm.type,
                taskForm.startDate,
                taskForm.deadline,
                taskForm.Attachments), Times.Once);
        }

        [Theory]
        [InlineData(System.Net.HttpStatusCode.BadRequest)]
        [InlineData(System.Net.HttpStatusCode.Unauthorized)]
        [InlineData(System.Net.HttpStatusCode.Forbidden)]
        [InlineData(System.Net.HttpStatusCode.NotFound)]
        [InlineData(System.Net.HttpStatusCode.Conflict)]
        public async Task CreateTask_WithDifferentErrorCodes_ReturnsCorrectStatusCode(System.Net.HttpStatusCode statusCode)
        {
            // Arrange
            var taskForm = new AddTaskForm
            {
                title = "Test Task",
                description = "Description",
                project = 1
            };

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(statusCode);

            MockTasksService.Setup(s => s.Create(
                It.IsAny<string>(),
                taskForm.title,
                taskForm.description,
                taskForm.asignees,
                taskForm.project,
                taskForm.priority,
                taskForm.type,
                taskForm.startDate,
                taskForm.deadline,
                taskForm.Attachments))
                .ReturnsAsync(error.Object);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.createTask(taskForm, MockConfiguration.Object);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)statusCode, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Update_WithMultiplePatches_CallsServiceCorrectly()
        {
            // Arrange
            const int taskId = 1;
            var patchDoc = new JsonPatchDocument<Tasks>();
            patchDoc.Replace(t => t.Title, "New Title");
            patchDoc.Replace(t => t.Description, "New Description");

            MockTasksService.Setup(s => s.Update(
                It.IsAny<string>(),
                taskId,
                patchDoc))
                .ReturnsAsync((IError?)null);

            // Act
            await _controller.update(taskId, patchDoc);

            // Assert
            MockTasksService.Verify(s => s.Update(
                It.IsAny<string>(),
                taskId,
                It.IsAny<JsonPatchDocument<Tasks>>()), Times.Once);
        }
    }
}
