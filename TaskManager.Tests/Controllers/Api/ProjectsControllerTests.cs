using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TaskManager.Controllers.Api;
using TaskManager.RequestForms;
using Application.Errors;
using OneOf;

namespace TaskManager.Tests.Controllers.Api
{
    public class ProjectsControllerTests : BaseControllerTest
    {
        private readonly ProjectsController _controller;

        public ProjectsControllerTests()
        {
            _controller = new ProjectsController(MockHttpContextAccessor.Object, MockProjectsService.Object);
            SetupControllerContext(_controller);
        }

        [Fact]
        public async Task GetProjectsEmployeesDetails_WithValidProjectId_ReturnsOkResult()
        {
            // Arrange
            const int projectId = 1;
            var employees = new List<object>
            {
                new { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
                new { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
            };

            MockProjectsService.Setup(s => s.GetProjectsEmployeesDetails(projectId))
                .ReturnsAsync(OneOf<IEnumerable<object>, IError>.FromT0(employees));

            // Act
            var result = await _controller.GetProjectsEmployeesDetails(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, returnedEmployees.Count());
        }

        [Fact]
        public async Task GetProjectsEmployeesDetails_WithError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int projectId = 999;
            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.NotFound);

            MockProjectsService.Setup(s => s.GetProjectsEmployeesDetails(projectId))
                .ReturnsAsync(OneOf<IEnumerable<object>, IError>.FromT1(error.Object));

            // Act
            var result = await _controller.GetProjectsEmployeesDetails(projectId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateProject_WithValidForm_ReturnsOkResult()
        {
            // Arrange
            var createProjectForm = new CreateProjectForm
            {
                Name = "New Project",
                Type = "Development",
                Description = "Project Description",
                StartDate = DateTime.Now,
                Deadline = DateTime.Now.AddMonths(3)
            };

            MockProjectsService.Setup(s => s.CreateProject(
                It.IsAny<bool>(), // isManager
                createProjectForm.Name,
                createProjectForm.Type,
                createProjectForm.Description,
                createProjectForm.StartDate,
                createProjectForm.Deadline))
                .ReturnsAsync((IError?)null);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.CreateProject(createProjectForm);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateProject_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var createProjectForm = new CreateProjectForm
            {
                Name = "", // Invalid - empty name
                Type = "Development",
                Description = "Description"
            };

            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.CreateProject(createProjectForm);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task CreateProject_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            var createProjectForm = new CreateProjectForm
            {
                Name = "Test Project",
                Type = "Development",
                Description = "Description",
                StartDate = DateTime.Now,
                Deadline = DateTime.Now.AddMonths(3)
            };

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.Unauthorized);

            MockProjectsService.Setup(s => s.CreateProject(
                It.IsAny<bool>(),
                createProjectForm.Name,
                createProjectForm.Type,
                createProjectForm.Description,
                createProjectForm.StartDate,
                createProjectForm.Deadline))
                .ReturnsAsync(error.Object);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.CreateProject(createProjectForm);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(401, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetProjectsTasksDetails_WithValidProjectId_ReturnsOkResult()
        {
            // Arrange
            const int projectId = 1;
            var tasks = new List<object>
            {
                new { Id = 1, Title = "Task 1", Description = "Description 1" },
                new { Id = 2, Title = "Task 2", Description = "Description 2" }
            };

            MockProjectsService.Setup(s => s.GetProjectsTasksDetails(projectId))
                .ReturnsAsync(OneOf<IEnumerable<object>, IError>.FromT0(tasks));

            // Act
            var result = await _controller.GetProjectsTasksDetails(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, returnedTasks.Count());
        }

        [Fact]
        public async Task GetProjectsTasksDetails_WithError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int projectId = 999;
            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.NotFound);

            MockProjectsService.Setup(s => s.GetProjectsTasksDetails(projectId))
                .ReturnsAsync(OneOf<IEnumerable<object>, IError>.FromT1(error.Object));

            // Act
            var result = await _controller.GetProjectsTasksDetails(projectId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetProjectsActivities_WithValidProjectId_ReturnsOkResult()
        {
            // Arrange
            const int projectId = 1;
            var activities = new List<object>
            {
                new { Id = 1, Action = "Created project", Timestamp = DateTime.Now },
                new { Id = 2, Action = "Added employee", Timestamp = DateTime.Now }
            };

            MockProjectsService.Setup(s => s.GetProjectsActivities(projectId))
                .ReturnsAsync(OneOf<IEnumerable<object>, IError>.FromT0(activities));

            // Act
            var result = await _controller.GetProjectsActivities(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActivities = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, returnedActivities.Count());
        }

        [Fact]
        public async Task GetProjectsActivities_WithError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int projectId = 999;
            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.NotFound);

            MockProjectsService.Setup(s => s.GetProjectsActivities(projectId))
                .ReturnsAsync(OneOf<IEnumerable<object>, IError>.FromT1(error.Object));

            // Act
            var result = await _controller.GetProjectsActivities(projectId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddEmployeeToProject_WithValidData_ReturnsOkResult()
        {
            // Arrange
            const int projectId = 1;
            var employeeIds = new List<int> { 1, 2, 3 };

            MockProjectsService.Setup(s => s.AddEmployeeToProject(
                It.IsAny<bool>(), // isManager
                projectId,
                employeeIds))
                .ReturnsAsync((IError?)null);

            // Act
            var result = await _controller.AddEmployeeToProject(projectId, employeeIds);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddEmployeeToProject_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int projectId = 1;
            var employeeIds = new List<int> { 1, 2 };

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.Forbidden);

            MockProjectsService.Setup(s => s.AddEmployeeToProject(
                It.IsAny<bool>(),
                projectId,
                employeeIds))
                .ReturnsAsync(error.Object);

            // Act
            var result = await _controller.AddEmployeeToProject(projectId, employeeIds);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_WithValidData_ReturnsOkResult()
        {
            // Arrange
            const int projectId = 1;
            const int employeeId = 1;

            MockProjectsService.Setup(s => s.DeleteEmployee(
                It.IsAny<bool>(), // isManager
                projectId,
                employeeId))
                .ReturnsAsync((IError?)null);

            // Act
            var result = await _controller.DeleteEmployee(projectId, employeeId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_WithServiceError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int projectId = 1;
            const int employeeId = 999;

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.NotFound);

            MockProjectsService.Setup(s => s.DeleteEmployee(
                It.IsAny<bool>(),
                projectId,
                employeeId))
                .ReturnsAsync(error.Object);

            // Act
            var result = await _controller.DeleteEmployee(projectId, employeeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task GetProjectsEmployeesDetails_WithDifferentProjectIds_CallsServiceCorrectly(int projectId)
        {
            // Arrange
            var employees = new List<object>();
            MockProjectsService.Setup(s => s.GetProjectsEmployeesDetails(projectId))
                .ReturnsAsync(OneOf<IEnumerable<object>, IError>.FromT0(employees));

            // Act
            await _controller.GetProjectsEmployeesDetails(projectId);

            // Assert
            MockProjectsService.Verify(s => s.GetProjectsEmployeesDetails(projectId), Times.Once);
        }

        [Fact]
        public async Task AddEmployeeToProject_WithEmptyEmployeeList_CallsService()
        {
            // Arrange
            const int projectId = 1;
            var employeeIds = new List<int>();

            MockProjectsService.Setup(s => s.AddEmployeeToProject(
                It.IsAny<bool>(),
                projectId,
                employeeIds))
                .ReturnsAsync((IError?)null);

            // Act
            await _controller.AddEmployeeToProject(projectId, employeeIds);

            // Assert
            MockProjectsService.Verify(s => s.AddEmployeeToProject(
                It.IsAny<bool>(),
                projectId,
                employeeIds), Times.Once);
        }

        [Theory]
        [InlineData(System.Net.HttpStatusCode.BadRequest)]
        [InlineData(System.Net.HttpStatusCode.Unauthorized)]
        [InlineData(System.Net.HttpStatusCode.Forbidden)]
        [InlineData(System.Net.HttpStatusCode.NotFound)]
        [InlineData(System.Net.HttpStatusCode.Conflict)]
        public async Task CreateProject_WithDifferentErrorCodes_ReturnsCorrectStatusCode(System.Net.HttpStatusCode statusCode)
        {
            // Arrange
            var createProjectForm = new CreateProjectForm
            {
                Name = "Test Project",
                Type = "Development",
                Description = "Description",
                StartDate = DateTime.Now,
                Deadline = DateTime.Now.AddMonths(3)
            };

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(statusCode);

            MockProjectsService.Setup(s => s.CreateProject(
                It.IsAny<bool>(),
                createProjectForm.Name,
                createProjectForm.Type,
                createProjectForm.Description,
                createProjectForm.StartDate,
                createProjectForm.Deadline))
                .ReturnsAsync(error.Object);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.CreateProject(createProjectForm);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)statusCode, statusCodeResult.StatusCode);
        }
    }
}
