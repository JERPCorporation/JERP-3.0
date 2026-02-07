using JERP.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Text.Json;

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
        var json = JsonSerializer.Serialize(value);
        var doc = JsonDocument.Parse(json);
        doc.RootElement.GetProperty("status").GetString().Should().Be("Healthy");
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
        var json = JsonSerializer.Serialize(value);
        var doc = JsonDocument.Parse(json);
        doc.RootElement.GetProperty("version").GetString().Should().Be("1.0.0");
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
        var json = JsonSerializer.Serialize(value);
        var doc = JsonDocument.Parse(json);
        doc.RootElement.GetProperty("developer").GetString().Should().Be("Julio Cesar Mendez Tobar Jr.");
        doc.RootElement.GetProperty("contact").GetString().Should().Be("ichbincesartobar@yahoo.com");
    }
}
