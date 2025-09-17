using Application.Errors.Authorizations;
using Application.Errors.Invites;
using Application.Services.Interfaces;
using Domain.Base;
using Domain.DomainModels.Employee;
using Domain.Employee;
using infrastructure.Services;
using Moq;
using Xunit;

namespace TaskManager.Tests.Services
{
    public class InviteServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEmployeesRepository> _mockEmployeesRepository;
        private readonly Mock<IInvitesRepository> _mockInvitesRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly InviteService _inviteService;

        public InviteServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEmployeesRepository = new Mock<IEmployeesRepository>();
            _mockInvitesRepository = new Mock<IInvitesRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _inviteService = new InviteService(_mockUnitOfWork.Object, _mockEmployeesRepository.Object, _mockInvitesRepository.Object, _mockEmailService.Object);
        }

        [Fact]
        public async Task InviteEmployee_ShouldReturnManagerAuthorizationError_WhenUserIsNotManager()
        {
            // Arrange
            bool isManager = false;
            int userId = 1;
            string host = "localhost";
            string email = "test@example.com";
            bool asManager = false;

            // Act
            var result = await _inviteService.InviteEmployee(isManager, userId, host, email, asManager);

            // Assert
            Assert.IsType<ManagerAuthorizationError>(result);
        }

        [Fact]
        public async Task InviteEmployee_ShouldReturnEmailAlreadyExistsError_WhenEmployeeExists()
        {
            // Arrange
            bool isManager = true;
            int userId = 1;
            string host = "localhost";
            string email = "test@example.com";
            bool asManager = false;
            var existingEmployee = new Employees(false, "Test", "User", "123456789", email, "password", "Developer", DateTime.Now);

            _mockEmployeesRepository.Setup(x => x.GetByEmail(email))
                .ReturnsAsync(existingEmployee);

            // Act
            var result = await _inviteService.InviteEmployee(isManager, userId, host, email, asManager);

            // Assert
            Assert.IsType<EmailAlreadyExistsError>(result);
            _mockEmployeesRepository.Verify(x => x.GetByEmail(email), Times.Once);
        }

        [Fact]
        public async Task InviteEmployee_ShouldReturnAlreadyInvitedError_WhenInviteExists()
        {
            // Arrange
            bool isManager = true;
            int userId = 1;
            string host = "localhost";
            string email = "test@example.com";
            bool asManager = false;
            var existingInvite = new Invites { inviteeEmail = email };

            _mockEmployeesRepository.Setup(x => x.GetByEmail(email))
                .ReturnsAsync((Employees)null);
            _mockInvitesRepository.Setup(x => x.GetByEmail(email))
                .ReturnsAsync(existingInvite);

            // Act
            var result = await _inviteService.InviteEmployee(isManager, userId, host, email, asManager);

            // Assert
            Assert.IsType<AlreadyInvitedError>(result);
            _mockEmployeesRepository.Verify(x => x.GetByEmail(email), Times.Once);
            _mockInvitesRepository.Verify(x => x.GetByEmail(email), Times.Once);
        }

        [Fact]
        public async Task InviteEmployee_ShouldSendInviteAndReturnNull_WhenValidRequest()
        {
            // Arrange
            bool isManager = true;
            int userId = 1;
            string host = "localhost";
            string email = "test@example.com";
            bool asManager = false;
            var inviter = new Employees(false, "John", "Doe", "123456789", "john@test.com", "password", "Manager", DateTime.Now);

            _mockEmployeesRepository.Setup(x => x.GetByEmail(email))
                .ReturnsAsync((Employees)null);
            _mockInvitesRepository.Setup(x => x.GetByEmail(email))
                .ReturnsAsync((Invites)null);
            _mockEmployeesRepository.Setup(x => x.GetEmployee(userId))
                .ReturnsAsync(inviter);
            _mockInvitesRepository.Setup(x => x.CreateInvite(It.IsAny<Invites>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _inviteService.InviteEmployee(isManager, userId, host, email, asManager);

            // Assert
            Assert.Null(result);
            _mockEmployeesRepository.Verify(x => x.GetByEmail(email), Times.Once);
            _mockInvitesRepository.Verify(x => x.GetByEmail(email), Times.Once);
            _mockEmployeesRepository.Verify(x => x.GetEmployee(userId), Times.Once);
            _mockEmailService.Verify(x => x.SendInviteEmail(inviter, email, It.IsAny<string>(), host), Times.Once);
            _mockInvitesRepository.Verify(x => x.CreateInvite(It.IsAny<Invites>()), Times.Once);
        }

        [Fact]
        public async Task GetInvite_ShouldReturnInvite_WhenFound()
        {
            // Arrange
            string email = "test@example.com";
            string secretKey = "testkey";
            var expectedInvite = new Invites { inviteeEmail = email, SecretKey = "hashedkey" };

            _mockInvitesRepository.Setup(x => x.GetByEmailAndSecretKey(email, It.IsAny<string>()))
                .ReturnsAsync(expectedInvite);

            // Act
            var result = await _inviteService.GetInvite(email, secretKey);

            // Assert
            Assert.Equal(expectedInvite, result);
            _mockInvitesRepository.Verify(x => x.GetByEmailAndSecretKey(email, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetInvite_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            string email = "test@example.com";
            string secretKey = "testkey";

            _mockInvitesRepository.Setup(x => x.GetByEmailAndSecretKey(email, It.IsAny<string>()))
                .ReturnsAsync((Invites)null);

            // Act
            var result = await _inviteService.GetInvite(email, secretKey);

            // Assert
            Assert.Null(result);
            _mockInvitesRepository.Verify(x => x.GetByEmailAndSecretKey(email, It.IsAny<string>()), Times.Once);
        }
    }
}
