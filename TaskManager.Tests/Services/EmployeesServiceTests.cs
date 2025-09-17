using Application.Errors.Employees;
using Application.Services.Interfaces;
using Domain.Base;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.Employee;
using Domain.Task;
using infrastructure.Services;
using Moq;
using OneOf;
using Xunit;

namespace TaskManager.Tests.Services
{
    public class EmployeesServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProjectsRepository> _mockProjectsRepository;
        private readonly Mock<IEmployeesRepository> _mockEmployeesRepository;
        private readonly EmployeesService _employeesService;

        public EmployeesServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProjectsRepository = new Mock<IProjectsRepository>();
            _mockEmployeesRepository = new Mock<IEmployeesRepository>();
            _employeesService = new EmployeesService(_mockUnitOfWork.Object, _mockProjectsRepository.Object, _mockEmployeesRepository.Object);
        }

        [Fact]
        public async Task Search_ShouldReturnEmployees_WhenCalled()
        {
            // Arrange
            var expectedEmployees = new List<Employees> 
            { 
                new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Developer", DateTime.Now),
                new Employees(false, "Jane", "Smith", "987654321", "jane@test.com", "password", "Designer", DateTime.Now)
            };
            _mockEmployeesRepository.Setup(x => x.GetAll("test", null, 1))
                .ReturnsAsync(expectedEmployees);

            // Act
            var result = await _employeesService.Search("test", 1);

            // Assert
            Assert.Equal(expectedEmployees, result);
            _mockEmployeesRepository.Verify(x => x.GetAll("test", null, 1), Times.Once);
        }

        [Fact]
        public async Task ExeludeEmployees_ShouldReturnExcludedEmployees_WhenCalled()
        {
            // Arrange
            var expectedEmployees = new List<object> { new { Id = 1, Name = "John Doe" } };
            _mockEmployeesRepository.Setup(x => x.ExeludeEmployees(1, "test"))
                .ReturnsAsync(expectedEmployees);

            // Act
            var result = await _employeesService.ExeludeEmployees(1, "test");

            // Assert
            Assert.Equal(expectedEmployees, result);
            _mockEmployeesRepository.Verify(x => x.ExeludeEmployees(1, "test"), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeTasks_ShouldReturnTasks_WhenEmployeeExists()
        {
            // Arrange
            var employee = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Developer", DateTime.Now);
            var expectedTasks = new List<Tasks> 
            { 
                new Tasks("Task 1", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature"),
                new Tasks("Task 2", DateTime.Now, DateTime.Now.AddDays(14), "Medium", "Description", "In Progress", "Bug")
            };

            _mockEmployeesRepository.Setup(x => x.GetEmployee(1))
                .ReturnsAsync(employee);
            _mockEmployeesRepository.Setup(x => x.GetEmployeeTasks(employee, false))
                .ReturnsAsync(expectedTasks);

            // Act
            var result = await _employeesService.GetEmployeeTasks(1, false);

            // Assert
            Assert.True(result.IsT0);
            Assert.Equal(expectedTasks, result.AsT0);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(1), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployeeTasks(employee, false), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeTasks_ShouldReturnError_WhenEmployeeNotFound()
        {
            // Arrange
            _mockEmployeesRepository.Setup(x => x.GetEmployee(1))
                .ReturnsAsync((Employees)null);

            // Act
            var result = await _employeesService.GetEmployeeTasks(1, false);

            // Assert
            Assert.True(result.IsT1);
            Assert.IsType<EmployeeNotFoundError>(result.AsT1);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(1), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployeeTasks(It.IsAny<Employees>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task GetEmployeeActivities_ShouldReturnActivities_WhenEmployeeExists()
        {
            // Arrange
            var employee = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Developer", DateTime.Now);
            var expectedActivities = new List<object> { new { Activity = "Completed Task 1" } };

            _mockEmployeesRepository.Setup(x => x.GetEmployee(1))
                .ReturnsAsync(employee);
            _mockEmployeesRepository.Setup(x => x.GetEmployeeActivities(employee))
                .ReturnsAsync(expectedActivities);

            // Act
            var result = await _employeesService.GetEmployeeActivities(1);

            // Assert
            Assert.True(result.IsT0);
            Assert.Equal(expectedActivities, result.AsT0);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(1), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployeeActivities(employee), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeActivities_ShouldReturnError_WhenEmployeeNotFound()
        {
            // Arrange
            _mockEmployeesRepository.Setup(x => x.GetEmployee(1))
                .ReturnsAsync((Employees)null);

            // Act
            var result = await _employeesService.GetEmployeeActivities(1);

            // Assert
            Assert.True(result.IsT1);
            Assert.IsType<EmployeeNotFoundError>(result.AsT1);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(1), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployeeActivities(It.IsAny<Employees>()), Times.Never);
        }
    }
}
