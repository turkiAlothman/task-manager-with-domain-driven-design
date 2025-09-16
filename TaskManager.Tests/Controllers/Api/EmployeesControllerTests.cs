using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TaskManager.Controllers.Api;
using Domain.Employee;
using Domain.Task;
using Application.Errors;
using OneOf;

namespace TaskManager.Tests.Controllers.Api
{
    public class EmployeesControllerTests : BaseControllerTest
    {
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _controller = new EmployeesController(MockEmployeesService.Object);
            SetupControllerContext(_controller);
        }

        [Fact]
        public async Task Search_WithDefaultParameters_ReturnsJsonResult()
        {
            // Arrange
            var employees = new List<Employees>
            {
                new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
                new() { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
            };

            MockEmployeesService.Setup(s => s.Search("", null))
                .ReturnsAsync(employees);

            // Act
            var result = await _controller.Search();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<Employees>>(jsonResult.Value);
            Assert.Equal(2, returnedEmployees.Count());
        }

        [Fact]
        public async Task Search_WithSearchTerm_ReturnsFilteredResults()
        {
            // Arrange
            const string searchTerm = "John";
            var employees = new List<Employees>
            {
                new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" }
            };

            MockEmployeesService.Setup(s => s.Search(searchTerm, null))
                .ReturnsAsync(employees);

            // Act
            var result = await _controller.Search(searchTerm);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<Employees>>(jsonResult.Value);
            Assert.Single(returnedEmployees);
        }

        [Fact]
        public async Task Search_WithProjectId_ReturnsProjectEmployees()
        {
            // Arrange
            const int projectId = 1;
            var employees = new List<Employees>
            {
                new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" }
            };

            MockEmployeesService.Setup(s => s.Search("", projectId))
                .ReturnsAsync(employees);

            // Act
            var result = await _controller.Search("", projectId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<Employees>>(jsonResult.Value);
            Assert.Single(returnedEmployees);
        }

        [Fact]
        public async Task ExeludeEmployees_WithValidProjectId_ReturnsJsonResult()
        {
            // Arrange
            const int projectId = 1;
            var excludedEmployees = new List<object>
            {
                new { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" }
            };

            MockEmployeesService.Setup(s => s.ExeludeEmployees(projectId, null))
                .ReturnsAsync(excludedEmployees);

            // Act
            var result = await _controller.ExeludeEmployees(projectId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<object>>(jsonResult.Value);
            Assert.Single(returnedEmployees);
        }

        [Fact]
        public async Task ExeludeEmployees_WithSearchTerm_ReturnsFilteredResults()
        {
            // Arrange
            const int projectId = 1;
            const string search = "John";
            var excludedEmployees = new List<object>
            {
                new { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" }
            };

            MockEmployeesService.Setup(s => s.ExeludeEmployees(projectId, search))
                .ReturnsAsync(excludedEmployees);

            // Act
            var result = await _controller.ExeludeEmployees(projectId, search);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var returnedEmployees = Assert.IsAssignableFrom<IEnumerable<object>>(jsonResult.Value);
            Assert.Single(returnedEmployees);
        }

        [Fact]
        public async Task GetEmployeeTasks_WithValidId_ReturnsOkResult()
        {
            // Arrange
            const int employeeId = 1;
            var tasks = new List<Tasks>
            {
                new() { Id = 1, Title = "Task 1", Description = "Description 1" },
                new() { Id = 2, Title = "Task 2", Description = "Description 2" }
            };

            MockEmployeesService.Setup(s => s.GetEmployeeTasks(employeeId, false))
                .ReturnsAsync(OneOf<IEnumerable<Tasks>, IError>.FromT0(tasks));

            // Act
            var result = await _controller.GetEmployeeTasks(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<Tasks>>(okResult.Value);
            Assert.Equal(2, returnedTasks.Count());
        }

        [Fact]
        public async Task GetEmployeeTasks_WithReportedFlag_ReturnsReportedTasks()
        {
            // Arrange
            const int employeeId = 1;
            const bool reported = true;
            var tasks = new List<Tasks>
            {
                new() { Id = 1, Title = "Reported Task", Description = "Description" }
            };

            MockEmployeesService.Setup(s => s.GetEmployeeTasks(employeeId, reported))
                .ReturnsAsync(OneOf<IEnumerable<Tasks>, IError>.FromT0(tasks));

            // Act
            var result = await _controller.GetEmployeeTasks(employeeId, reported);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<Tasks>>(okResult.Value);
            Assert.Single(returnedTasks);
        }

        [Fact]
        public async Task GetEmployeeTasks_WithError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int employeeId = 999;
            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.NotFound);

            MockEmployeesService.Setup(s => s.GetEmployeeTasks(employeeId, false))
                .ReturnsAsync(OneOf<IEnumerable<Tasks>, IError>.FromT1(error.Object));

            // Act
            var result = await _controller.GetEmployeeTasks(employeeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetEmployeeActivities_WithValidId_ReturnsOkResult()
        {
            // Arrange
            const int employeeId = 1;
            var activities = new List<object>
            {
                new { Id = 1, Action = "Created task", Timestamp = DateTime.Now },
                new { Id = 2, Action = "Updated task", Timestamp = DateTime.Now }
            };

            MockEmployeesService.Setup(s => s.GetEmployeeActivities(employeeId))
                .ReturnsAsync(OneOf<IEnumerable<object>, IError>.FromT0(activities));

            // Act
            var result = await _controller.GetEmployeeActivities(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActivities = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, returnedActivities.Count());
        }

        [Fact]
        public async Task GetEmployeeActivities_WithError_ReturnsStatusCodeResult()
        {
            // Arrange
            const int employeeId = 999;
            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.NotFound);

            MockEmployeesService.Setup(s => s.GetEmployeeActivities(employeeId))
                .ReturnsAsync(OneOf<IEnumerable<object>, IError>.FromT1(error.Object));

            // Act
            var result = await _controller.GetEmployeeActivities(employeeId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("john")]
        [InlineData("Jane Smith")]
        public async Task Search_WithVariousSearchTerms_CallsServiceCorrectly(string searchTerm)
        {
            // Arrange
            var employees = new List<Employees>();
            MockEmployeesService.Setup(s => s.Search(searchTerm, null))
                .ReturnsAsync(employees);

            // Act
            await _controller.Search(searchTerm);

            // Assert
            MockEmployeesService.Verify(s => s.Search(searchTerm, null), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task ExeludeEmployees_WithDifferentProjectIds_CallsServiceCorrectly(int projectId)
        {
            // Arrange
            var excludedEmployees = new List<object>();
            MockEmployeesService.Setup(s => s.ExeludeEmployees(projectId, null))
                .ReturnsAsync(excludedEmployees);

            // Act
            await _controller.ExeludeEmployees(projectId);

            // Assert
            MockEmployeesService.Verify(s => s.ExeludeEmployees(projectId, null), Times.Once);
        }
    }
}
