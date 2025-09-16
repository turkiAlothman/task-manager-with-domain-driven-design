using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using TaskManager.Controllers.Api;
using TaskManager.RequestForms;
using Application.Errors;
using Microsoft.AspNetCore.Http;

namespace TaskManager.Tests.Controllers.Api
{
    public class InviteControllerTests : BaseControllerTest
    {
        private readonly InviteController _controller;

        public InviteControllerTests()
        {
            _controller = new InviteController(MockHttpContextAccessor.Object, MockInviteService.Object);
            SetupControllerContext(_controller);
        }

        [Fact]
        public async Task InviteEmployee_WithValidForm_ReturnsOkResult()
        {
            // Arrange
            var inviteForm = new InviteForm
            {
                email = "newuser@example.com",
                AsManager = false
            };

            MockInviteService.Setup(s => s.InviteEmployee(
                It.IsAny<bool>(), // isManager
                It.IsAny<string>(), // userId
                It.IsAny<string>(), // host
                inviteForm.email,
                inviteForm.AsManager))
                .ReturnsAsync((IError?)null);

            _controller.ModelState.Clear(); // Ensure ModelState is valid

            // Act
            var result = await _controller.InviteEmployee(inviteForm);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Assert.NotNull(response);
        }

        [Fact]
        public async Task InviteEmployee_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var inviteForm = new InviteForm
            {
                email = "invalid-email", // Invalid email format
                AsManager = false
            };

            _controller.ModelState.AddModelError("email", "Invalid email format");

            // Act
            var result = await _controller.InviteEmployee(inviteForm);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task InviteEmployee_WithServiceError_ReturnsStatusCode()
        {
            // Arrange
            var inviteForm = new InviteForm
            {
                email = "existing@example.com",
                AsManager = false
            };

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(System.Net.HttpStatusCode.Conflict);

            MockInviteService.Setup(s => s.InviteEmployee(
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                inviteForm.email,
                inviteForm.AsManager))
                .ReturnsAsync(error.Object);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.InviteEmployee(inviteForm);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(409, statusCodeResult.StatusCode);
            Assert.Equal(error.Object, statusCodeResult.Value);
        }

        [Fact]
        public async Task InviteEmployee_AsManager_CallsServiceWithCorrectParameters()
        {
            // Arrange
            var inviteForm = new InviteForm
            {
                email = "manager@example.com",
                AsManager = true
            };

            MockInviteService.Setup(s => s.InviteEmployee(
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                inviteForm.email,
                inviteForm.AsManager))
                .ReturnsAsync((IError?)null);

            _controller.ModelState.Clear();

            // Act
            await _controller.InviteEmployee(inviteForm);

            // Assert
            MockInviteService.Verify(s => s.InviteEmployee(
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                inviteForm.email,
                true), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task InviteEmployee_WithEmptyEmail_ReturnsBadRequestWhenValidated(string email)
        {
            // Arrange
            var inviteForm = new InviteForm
            {
                email = email,
                AsManager = false
            };

            _controller.ModelState.AddModelError("email", "Email is required");

            // Act
            var result = await _controller.InviteEmployee(inviteForm);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task InviteEmployee_WithValidEmailFormat_PassesValidation()
        {
            // Arrange
            var inviteForm = new InviteForm
            {
                email = "valid.email@example.com",
                AsManager = false
            };

            MockInviteService.Setup(s => s.InviteEmployee(
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                inviteForm.email,
                inviteForm.AsManager))
                .ReturnsAsync((IError?)null);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.InviteEmployee(inviteForm);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task InviteEmployee_ChecksHttpContextForUserInfo()
        {
            // Arrange
            var inviteForm = new InviteForm
            {
                email = "test@example.com",
                AsManager = false
            };

            MockInviteService.Setup(s => s.InviteEmployee(
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                inviteForm.email,
                inviteForm.AsManager))
                .ReturnsAsync((IError?)null);

            _controller.ModelState.Clear();

            // Act
            await _controller.InviteEmployee(inviteForm);

            // Assert
            MockHttpContextAccessor.Verify(x => x.HttpContext, Times.AtLeastOnce);
        }

        [Theory]
        [InlineData(System.Net.HttpStatusCode.BadRequest)]
        [InlineData(System.Net.HttpStatusCode.Unauthorized)]
        [InlineData(System.Net.HttpStatusCode.Forbidden)]
        [InlineData(System.Net.HttpStatusCode.NotFound)]
        [InlineData(System.Net.HttpStatusCode.Conflict)]
        public async Task InviteEmployee_WithDifferentErrorCodes_ReturnsCorrectStatusCode(System.Net.HttpStatusCode statusCode)
        {
            // Arrange
            var inviteForm = new InviteForm
            {
                email = "test@example.com",
                AsManager = false
            };

            var error = new Mock<IError>();
            error.Setup(e => e.StatusCode).Returns(statusCode);

            MockInviteService.Setup(s => s.InviteEmployee(
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                inviteForm.email,
                inviteForm.AsManager))
                .ReturnsAsync(error.Object);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.InviteEmployee(inviteForm);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)statusCode, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task InviteEmployee_SuccessResponse_ContainsExpectedMessage()
        {
            // Arrange
            var inviteForm = new InviteForm
            {
                email = "success@example.com",
                AsManager = false
            };

            MockInviteService.Setup(s => s.InviteEmployee(
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                inviteForm.email,
                inviteForm.AsManager))
                .ReturnsAsync((IError?)null);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.InviteEmployee(inviteForm);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Assert.NotNull(response);
            
            // Check if the response contains the expected message structure
            var responseType = response.GetType();
            var messageProperty = responseType.GetProperty("Message");
            Assert.NotNull(messageProperty);
            
            var messageValue = messageProperty.GetValue(response) as string;
            Assert.Equal("email invited successfully!", messageValue);
        }
    }
}
