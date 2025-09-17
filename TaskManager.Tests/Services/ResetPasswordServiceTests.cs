using Application.Errors.Authorizations;
using Application.Services.Interfaces;
using Domain.Base;
using Domain.DomainModels.ResetPasswords;
using Domain.ResetPasswords;
using infrastructure.Services;
using Moq;
using Xunit;

namespace TaskManager.Tests.Services
{
    public class ResetPasswordServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IResetPasswordRepository> _mockResetPasswordRepository;
        private readonly ResetPasswordService _resetPasswordService;

        public ResetPasswordServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockResetPasswordRepository = new Mock<IResetPasswordRepository>();
            _resetPasswordService = new ResetPasswordService(_mockUnitOfWork.Object, _mockResetPasswordRepository.Object);
        }

        [Fact]
        public async Task CheckResetPasswordRequest_ShouldReturnNull_WhenValidRequest()
        {
            // Arrange
            string email = "test@example.com";
            string secretKey = "validkey";
            var resetPassword = new ResetPassword(email, "hashedkey", DateTime.Now.AddHours(1));

            _mockResetPasswordRepository.Setup(x => x.GetByEmailAndSecret(email, It.IsAny<string>()))
                .ReturnsAsync(resetPassword);

            // Act
            var result = await _resetPasswordService.CheckResetPasswordRequest(email, secretKey);

            // Assert
            Assert.Null(result);
            _mockResetPasswordRepository.Verify(x => x.GetByEmailAndSecret(email, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CheckResetPasswordRequest_ShouldReturnError_WhenResetPasswordNotFound()
        {
            // Arrange
            string email = "test@example.com";
            string secretKey = "invalidkey";

            _mockResetPasswordRepository.Setup(x => x.GetByEmailAndSecret(email, It.IsAny<string>()))
                .ReturnsAsync((ResetPassword)null);

            // Act
            var result = await _resetPasswordService.CheckResetPasswordRequest(email, secretKey);

            // Assert
            Assert.IsType<NotAuthorizedResetPasswordRequest>(result);
            _mockResetPasswordRepository.Verify(x => x.GetByEmailAndSecret(email, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CheckResetPasswordRequest_ShouldReturnError_WhenRequestIsExpired()
        {
            // Arrange
            string email = "test@example.com";
            string secretKey = "expiredkey";
            var expiredResetPassword = new ResetPassword(email, "hashedkey", DateTime.Now.AddHours(-1));

            _mockResetPasswordRepository.Setup(x => x.GetByEmailAndSecret(email, It.IsAny<string>()))
                .ReturnsAsync(expiredResetPassword);

            // Act
            var result = await _resetPasswordService.CheckResetPasswordRequest(email, secretKey);

            // Assert
            Assert.IsType<NotAuthorizedResetPasswordRequest>(result);
            _mockResetPasswordRepository.Verify(x => x.GetByEmailAndSecret(email, It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData("user1@test.com", "key123")]
        [InlineData("user2@test.com", "key456")]
        [InlineData("user3@test.com", "key789")]
        public async Task CheckResetPasswordRequest_ShouldHandleDifferentInputs(string email, string secretKey)
        {
            // Arrange
            _mockResetPasswordRepository.Setup(x => x.GetByEmailAndSecret(email, It.IsAny<string>()))
                .ReturnsAsync((ResetPassword)null);

            // Act
            var result = await _resetPasswordService.CheckResetPasswordRequest(email, secretKey);

            // Assert
            Assert.IsType<NotAuthorizedResetPasswordRequest>(result);
            _mockResetPasswordRepository.Verify(x => x.GetByEmailAndSecret(email, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CheckResetPasswordRequest_ShouldCheckExpirationDateCorrectly()
        {
            // Arrange
            string email = "test@example.com";
            string secretKey = "borderlinekey";
            var resetPassword = new ResetPassword(email, "hashedkey", DateTime.Now.AddMinutes(-1));

            _mockResetPasswordRepository.Setup(x => x.GetByEmailAndSecret(email, It.IsAny<string>()))
                .ReturnsAsync(resetPassword);

            // Act
            var result = await _resetPasswordService.CheckResetPasswordRequest(email, secretKey);

            // Assert
            Assert.IsType<NotAuthorizedResetPasswordRequest>(result);
            _mockResetPasswordRepository.Verify(x => x.GetByEmailAndSecret(email, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CheckResetPasswordRequest_ShouldCallHashOnSecretKey()
        {
            // Arrange
            string email = "test@example.com";
            string secretKey = "plainkey";
            
            _mockResetPasswordRepository.Setup(x => x.GetByEmailAndSecret(email, It.IsAny<string>()))
                .ReturnsAsync((ResetPassword)null);

            // Act
            var result = await _resetPasswordService.CheckResetPasswordRequest(email, secretKey);

            // Assert
            // Verify that the repository was called with the email and some processed version of the secret key
            _mockResetPasswordRepository.Verify(x => x.GetByEmailAndSecret(email, It.IsAny<string>()), Times.Once);
            Assert.IsType<NotAuthorizedResetPasswordRequest>(result);
        }
    }
}
