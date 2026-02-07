using JERP.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace JERP.Api.Tests.Controllers;

public class HealthControllerTests
{
    [Fact]
    public void GetHealth_ShouldReturnOkResult()
    {
        // Arrange
        var controller = new HealthController();

        // Act
        var result = controller.GetHealth();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void GetHealth_ShouldReturnHealthyStatus()
    {
        // Arrange
        var controller = new HealthController();

        // Act
        var result = controller.GetHealth() as OkObjectResult;
        var value = result?.Value;

        // Assert
        value.Should().NotBeNull();
        var json = System.Text.Json.JsonSerializer.Serialize(value);
        json.Should().Contain("\"status\":\"Healthy\"");
    }

    [Fact]
    public void GetHealth_ShouldReturnVersion()
    {
        // Arrange
        var controller = new HealthController();

        // Act
        var result = controller.GetHealth() as OkObjectResult;
        var value = result?.Value;

        // Assert
        value.Should().NotBeNull();
        var json = System.Text.Json.JsonSerializer.Serialize(value);
        json.Should().Contain("\"version\":\"1.0.0\"");
    }

    [Fact]
    public void GetHealth_ShouldReturnDeveloperInfo()
    {
        // Arrange
        var controller = new HealthController();

        // Act
        var result = controller.GetHealth() as OkObjectResult;
        var value = result?.Value;

        // Assert
        value.Should().NotBeNull();
        var json = System.Text.Json.JsonSerializer.Serialize(value);
        json.Should().Contain("\"developer\":\"Julio Cesar Mendez Tobar Jr.\"");
        json.Should().Contain("\"contact\":\"ichbincesartobar@yahoo.com\"");
    }
}
