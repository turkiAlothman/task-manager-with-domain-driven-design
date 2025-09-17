using Application.Errors;
using Application.Errors.Authorizations;
using Application.Errors.Employees;
using Application.Errors.Tasks;
using Application.Services.Interfaces;
using Domain.Base;
using Domain.DTOs;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.DomainModels.Task;
using Domain.Employee;
using Domain.Project;
using Domain.Task;
using infrastructure.Services;
using Moq;
using OneOf;
using Xunit;

namespace TaskManager.Tests.Services
{
    public class ProjectsServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProjectsRepository> _mockProjectsRepository;
        private readonly Mock<ITasksRepository> _mockTasksRepository;
        private readonly Mock<IEmployeesRepository> _mockEmployeesRepository;
        private readonly ProjectsService _projectsService;

        public ProjectsServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProjectsRepository = new Mock<IProjectsRepository>();
            _mockTasksRepository = new Mock<ITasksRepository>();
            _mockEmployeesRepository = new Mock<IEmployeesRepository>();
            _projectsService = new ProjectsService(_mockUnitOfWork.Object, _mockProjectsRepository.Object, _mockTasksRepository.Object, _mockEmployeesRepository.Object);
        }

        [Fact]
        public async Task GetProjectsEmployeesDetails_ShouldReturnEmployees_WhenProjectExists()
        {
            // Arrange
            int projectId = 1;
            var expectedEmployees = new List<EmployeesDetailsWithinProjectResposne>
            {
                new EmployeesDetailsWithinProjectResposne { id = 1, FirstName = "John", LastName = "Doe" },
                new EmployeesDetailsWithinProjectResposne { id = 2, FirstName = "Jane", LastName = "Smith" }
            };

            _mockProjectsRepository.Setup(x => x.GetProjectsEmployeesDetails(projectId))
                .ReturnsAsync(expectedEmployees);

            // Act
            var result = await _projectsService.GetProjectsEmployeesDetails(projectId);

            // Assert
            Assert.True(result.IsT0);
            Assert.Equal(expectedEmployees, result.AsT0);
            _mockProjectsRepository.Verify(x => x.GetProjectsEmployeesDetails(projectId), Times.Once);
        }

        [Fact]
        public async Task GetProjectsEmployeesDetails_ShouldReturnError_WhenProjectNotFound()
        {
            // Arrange
            int projectId = 1;
            _mockProjectsRepository.Setup(x => x.GetProjectsEmployeesDetails(projectId))
                .ReturnsAsync((IEnumerable<EmployeesDetailsWithinProjectResposne>)null);

            // Act
            var result = await _projectsService.GetProjectsEmployeesDetails(projectId);

            // Assert
            Assert.True(result.IsT1);
            Assert.IsType<ProjectNotFoundError>(result.AsT1);
            _mockProjectsRepository.Verify(x => x.GetProjectsEmployeesDetails(projectId), Times.Once);
        }

        [Fact]
        public async Task CreateProject_ShouldReturnManagerAuthorizationError_WhenUserIsNotManager()
        {
            // Arrange
            bool isManager = false;
            string name = "Test Project";
            string type = "Web App";
            string description = "Test Description";
            DateTime startDate = DateTime.Now;
            DateTime dueDate = DateTime.Now.AddMonths(3);

            // Act
            var result = await _projectsService.CreateProject(isManager, name, type, description, startDate, dueDate);

            // Assert
            Assert.IsType<ManagerAuthorizationError>(result);
            _mockProjectsRepository.Verify(x => x.CreateProject(It.IsAny<Projects>()), Times.Never);
        }

        [Fact]
        public async Task CreateProject_ShouldCreateProject_WhenUserIsManager()
        {
            // Arrange
            bool isManager = true;
            string name = "Test Project";
            string type = "Web App";
            string description = "Test Description";
            DateTime startDate = DateTime.Now;
            DateTime dueDate = DateTime.Now.AddMonths(3);

            _mockProjectsRepository.Setup(x => x.CreateProject(It.IsAny<Projects>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _projectsService.CreateProject(isManager, name, type, description, startDate, dueDate);

            // Assert
            Assert.Null(result);
            _mockProjectsRepository.Verify(x => x.CreateProject(It.Is<Projects>(p => 
                p.Name == name && p.Type == type && p.Description == description)), Times.Once);
        }

        [Fact]
        public async Task GetProjectsTasksDetails_ShouldReturnTasks_WhenProjectExists()
        {
            // Arrange
            int projectId = 1;
            var project = new Projects("Test Project", "Web App", "Description", DateTime.Now, DateTime.Now.AddMonths(3));
            var expectedTasks = new List<Tasks>
            {
                new Tasks("Task 1", DateTime.Now, DateTime.Now.AddDays(7), "High", "Description", "Open", "Feature"),
                new Tasks("Task 2", DateTime.Now, DateTime.Now.AddDays(14), "Medium", "Description", "In Progress", "Bug")
            };

            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync(project);
            _mockTasksRepository.Setup(x => x.GetByProject(project))
                .ReturnsAsync(expectedTasks);

            // Act
            var result = await _projectsService.GetProjectsTasksDetails(projectId);

            // Assert
            Assert.True(result.IsT0);
            Assert.Equal(expectedTasks, result.AsT0);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
            _mockTasksRepository.Verify(x => x.GetByProject(project), Times.Once);
        }

        [Fact]
        public async Task GetProjectsTasksDetails_ShouldReturnError_WhenProjectNotFound()
        {
            // Arrange
            int projectId = 1;
            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync((Projects)null);

            // Act
            var result = await _projectsService.GetProjectsTasksDetails(projectId);

            // Assert
            Assert.True(result.IsT1);
            Assert.IsType<ProjectNotFoundError>(result.AsT1);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
            _mockTasksRepository.Verify(x => x.GetByProject(It.IsAny<Projects>()), Times.Never);
        }

        [Fact]
        public async Task GetProjectsActivities_ShouldReturnActivities_WhenProjectExists()
        {
            // Arrange
            int projectId = 1;
            var project = new Projects("Test Project", "Web App", "Description", DateTime.Now, DateTime.Now.AddMonths(3));
            var expectedActivities = new List<ActivityRecord>
            {
                new ActivityRecord { description = "Task completed", CreatedAt = DateTime.Now }
            };

            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync(project);
            _mockProjectsRepository.Setup(x => x.GetProjectsActivities(project))
                .ReturnsAsync(expectedActivities);

            // Act
            var result = await _projectsService.GetProjectsActivities(projectId);

            // Assert
            Assert.True(result.IsT0);
            Assert.Equal(expectedActivities, result.AsT0);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
            _mockProjectsRepository.Verify(x => x.GetProjectsActivities(project), Times.Once);
        }

        [Fact]
        public async Task AddEmployeeToProject_ShouldReturnManagerAuthorizationError_WhenUserIsNotManager()
        {
            // Arrange
            bool isManager = false;
            int projectId = 1;
            var employeeIds = new List<int> { 1, 2 };

            // Act
            var result = await _projectsService.AddEmployeeToProject(isManager, projectId, employeeIds);

            // Assert
            Assert.IsType<ManagerAuthorizationError>(result);
        }

        [Fact]
        public async Task AddEmployeeToProject_ShouldReturnProjectNotFoundError_WhenProjectDoesNotExist()
        {
            // Arrange
            bool isManager = true;
            int projectId = 1;
            var employeeIds = new List<int> { 1, 2 };

            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync((Projects)null);

            // Act
            var result = await _projectsService.AddEmployeeToProject(isManager, projectId, employeeIds);

            // Assert
            Assert.IsType<ProjectNotFoundError>(result);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
        }

        [Fact]
        public async Task AddEmployeeToProject_ShouldAddEmployees_WhenValidRequest()
        {
            // Arrange
            bool isManager = true;
            int projectId = 1;
            var employeeIds = new List<int> { 1, 2 };
            var project = new Projects("Test Project", "Web App", "Description", DateTime.Now, DateTime.Now.AddMonths(3));
            var employees = new List<Employees>
            {
                new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Developer", DateTime.Now),
                new Employees(false, "Jane", "Smith", "987654321", "jane@test.com", "password", "Designer", DateTime.Now)
            };

            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync(project);
            _mockEmployeesRepository.Setup(x => x.GetEmployeesByListOfIds(employeeIds, project))
                .ReturnsAsync(employees);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _projectsService.AddEmployeeToProject(isManager, projectId, employeeIds);

            // Assert
            Assert.Null(result);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployeesByListOfIds(employeeIds, project), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnAuthorizationError_WhenUserIsNotManager()
        {
            // Arrange
            bool isManager = false;
            int projectId = 1;
            int employeeId = 1;

            // Act
            var result = await _projectsService.DeleteEmployee(isManager, projectId, employeeId);

            // Assert
            Assert.IsType<UnathorizedTaskActionError>(result);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnProjectNotFoundError_WhenProjectDoesNotExist()
        {
            // Arrange
            bool isManager = true;
            int projectId = 1;
            int employeeId = 1;

            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync((Projects)null);

            // Act
            var result = await _projectsService.DeleteEmployee(isManager, projectId, employeeId);

            // Assert
            Assert.IsType<ProjectNotFoundError>(result);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnEmployeeNotFoundError_WhenEmployeeDoesNotExist()
        {
            // Arrange
            bool isManager = true;
            int projectId = 1;
            int employeeId = 1;
            var project = new Projects("Test Project", "Web App", "Description", DateTime.Now, DateTime.Now.AddMonths(3));

            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync(project);
            _mockEmployeesRepository.Setup(x => x.GetEmployee(employeeId))
                .ReturnsAsync((Employees)null);

            // Act
            var result = await _projectsService.DeleteEmployee(isManager, projectId, employeeId);

            // Assert
            Assert.IsType<EmployeeNotFoundError>(result);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(employeeId), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldRemoveEmployee_WhenValidRequest()
        {
            // Arrange
            bool isManager = true;
            int projectId = 1;
            int employeeId = 1;
            var project = new Projects("Test Project", "Web App", "Description", DateTime.Now, DateTime.Now.AddMonths(3));
            var employee = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Developer", DateTime.Now);

            _mockProjectsRepository.Setup(x => x.GetById(projectId))
                .ReturnsAsync(project);
            _mockEmployeesRepository.Setup(x => x.GetEmployee(employeeId))
                .ReturnsAsync(employee);
            _mockProjectsRepository.Setup(x => x.RemoveEmployee(project, employee))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _projectsService.DeleteEmployee(isManager, projectId, employeeId);

            // Assert
            Assert.Null(result);
            _mockProjectsRepository.Verify(x => x.GetById(projectId), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(employeeId), Times.Once);
            _mockProjectsRepository.Verify(x => x.RemoveEmployee(project, employee), Times.Once);
        }
    }
}
