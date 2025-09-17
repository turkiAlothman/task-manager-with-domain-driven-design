using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TaskManager.Controllers.Api;

namespace TaskManager.Tests.Controllers.Api
{
    public class SimpleEmployeesControllerTests : BaseControllerTest
    {
        private readonly EmployeesController _controller;

        public SimpleEmployeesControllerTests()
        {
            _controller = new EmployeesController(MockEmployeesService.Object);
            SetupControllerContext(_controller);
        }

        [Fact]
        public async Task Search_WithDefaultParameters_CallsService()
        {
            // Arrange
            var mockEmployees = new List<Domain.Employee.Employees>();
            MockEmployeesService.Setup(s => s.Search("", null))
                .ReturnsAsync(mockEmployees);

            // Act
            var result = await _controller.Search();

            // Assert
            Assert.IsType<JsonResult>(result);
            MockEmployeesService.Verify(s => s.Search("", null), Times.Once);
        }

        [Fact]
        public async Task Search_WithSearchTerm_CallsService()
        {
            // Arrange
            const string searchTerm = "John";
            var mockEmployees = new List<Domain.Employee.Employees>();
            MockEmployeesService.Setup(s => s.Search(searchTerm, null))
                .ReturnsAsync(mockEmployees);

            // Act
            var result = await _controller.Search(searchTerm);

            // Assert
            Assert.IsType<JsonResult>(result);
            MockEmployeesService.Verify(s => s.Search(searchTerm, null), Times.Once);
        }

        [Fact]
        public async Task ExeludeEmployees_WithValidProjectId_CallsService()
        {
            // Arrange
            const int projectId = 1;
            var mockEmployees = new List<object>();
            MockEmployeesService.Setup(s => s.ExeludeEmployees(projectId, null))
                .ReturnsAsync(mockEmployees);

            // Act
            var result = await _controller.ExeludeEmployees(projectId);

            // Assert
            Assert.IsType<JsonResult>(result);
            MockEmployeesService.Verify(s => s.ExeludeEmployees(projectId, null), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData("john")]
        [InlineData("Jane Smith")]
        public async Task Search_WithVariousTerms_WorksCorrectly(string searchTerm)
        {
            // Arrange
            var mockEmployees = new List<Domain.Employee.Employees>();
            MockEmployeesService.Setup(s => s.Search(searchTerm, null))
                .ReturnsAsync(mockEmployees);

            // Act
            var result = await _controller.Search(searchTerm);

            // Assert
            Assert.IsType<JsonResult>(result);
            MockEmployeesService.Verify(s => s.Search(searchTerm, null), Times.Once);
        }
    }
}
