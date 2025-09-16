using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Tests
{
    public abstract class BaseControllerTest
    {
        protected Mock<IEmployeesService> MockEmployeesService { get; }
        protected Mock<IInviteService> MockInviteService { get; }
        protected Mock<IProjectsService> MockProjectsService { get; }
        protected Mock<ITasksService> MockTasksService { get; }
        protected Mock<IHttpContextAccessor> MockHttpContextAccessor { get; }
        protected Mock<IConfiguration> MockConfiguration { get; }

        protected BaseControllerTest()
        {
            MockEmployeesService = new Mock<IEmployeesService>();
            MockInviteService = new Mock<IInviteService>();
            MockProjectsService = new Mock<IProjectsService>();
            MockTasksService = new Mock<ITasksService>();
            MockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            MockConfiguration = new Mock<IConfiguration>();
        }

        protected void SetupControllerContext(ControllerBase controller, string userId = "1", string email = "test@example.com")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email)
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            MockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        }
    }
}
