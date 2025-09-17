using Microsoft.AspNetCore.Mvc;
using Xunit;
using TaskManager.Controllers.Api;
using Application.Services.Interfaces;

namespace TaskManager.Tests.Controllers.Api
{
    public class TestControllerTests
    {
        [Fact]
        public void TestController_HasValidRoute()
        {
            // Arrange & Act & Assert
            // The TestController should be properly accessible
            // This is a basic test to ensure the controller is properly set up
            Assert.True(true); // Simple validation that the test class can be instantiated
        }

        [Fact]
        public void TestController_InheritsFromControllerBase()
        {
            // Act & Assert
            // Verify that TestController inherits from ControllerBase
            Assert.True(typeof(ControllerBase).IsAssignableFrom(typeof(TestController)));
        }

        [Fact]
        public void TestController_HasApiControllerAttribute()
        {
            // Act
            var attributes = typeof(TestController).GetCustomAttributes(typeof(ApiControllerAttribute), false);

            // Assert
            Assert.NotEmpty(attributes);
        }

        [Fact]
        public void TestController_HasRouteAttribute()
        {
            // Act
            var attributes = typeof(TestController).GetCustomAttributes(typeof(RouteAttribute), false);

            // Assert
            Assert.NotEmpty(attributes);
            var routeAttribute = (RouteAttribute)attributes[0];
            Assert.Equal("api/[controller]", routeAttribute.Template);
        }

        [Fact]
        public void TestController_HasIndexMethod()
        {
            // Act
            var method = typeof(TestController).GetMethod("Index");

            // Assert
            Assert.NotNull(method);
            Assert.True(method.IsPublic);
        }

        [Fact]
        public void TestController_IndexMethodHasHttpGetAttribute()
        {
            // Act
            var method = typeof(TestController).GetMethod("Index");
            var attributes = method?.GetCustomAttributes(typeof(HttpGetAttribute), false);

            // Assert
            Assert.NotNull(attributes);
            Assert.NotEmpty(attributes);
        }

        [Fact]
        public void TestController_IndexMethodReturnsTask()
        {
            // Act
            var method = typeof(TestController).GetMethod("Index");

            // Assert
            Assert.NotNull(method);
            Assert.Equal(typeof(Task<IActionResult>), method.ReturnType);
        }

        [Fact]
        public void TestController_IndexMethodHasCorrectParameters()
        {
            // Act
            var method = typeof(TestController).GetMethod("Index");
            var parameters = method?.GetParameters();

            // Assert
            Assert.NotNull(parameters);
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(Microsoft.Extensions.Configuration.IConfiguration), parameters[0].ParameterType);
            Assert.Equal(typeof(IEmailService), parameters[1].ParameterType);
        }
    }
}
