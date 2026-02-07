using JERP.Api.Controllers;
using JERP.Application.DTOs.Reports;
using JERP.Application.Services.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace JERP.Api.Tests.Controllers;

public class KPIControllerTests
{
    private readonly Mock<IDashboardService> _mockDashboardService;
    private readonly Mock<ILogger<KPIController>> _mockLogger;
    private readonly KPIController _controller;

    public KPIControllerTests()
    {
        _mockDashboardService = new Mock<IDashboardService>();
        _mockLogger = new Mock<ILogger<KPIController>>();
        _controller = new KPIController(_mockDashboardService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetKPIs_ShouldReturnOkResult_WithDashboardKPIs()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var asOfDate = DateTime.UtcNow;
        var expectedKPIs = new DashboardKPIDto
        {
            CompanyId = companyId,
            AsOfDate = asOfDate,
            TotalRevenueMTD = 45000.00m,
            TotalRevenueYTD = 150000.00m,
            CashBalance = 100000.00m,
            EmployeeCount = 50,
            TotalPayrollCostMTD = 25000.00m,
            GeneratedAt = DateTime.UtcNow
        };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, asOfDate))
            .ReturnsAsync(expectedKPIs);

        // Act
        var result = await _controller.GetKPIs(companyId, asOfDate);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedKPIs);
        
        _mockDashboardService.Verify(
            s => s.GetDashboardKPIsAsync(companyId, asOfDate), 
            Times.Once);
    }

    [Fact]
    public async Task GetKPIs_ShouldCallService_WithNullAsOfDate_WhenNotProvided()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var expectedKPIs = new DashboardKPIDto
        {
            CompanyId = companyId,
            AsOfDate = DateTime.UtcNow,
            TotalRevenueMTD = 45000.00m,
            GeneratedAt = DateTime.UtcNow
        };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, null))
            .ReturnsAsync(expectedKPIs);

        // Act
        var result = await _controller.GetKPIs(companyId, null);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockDashboardService.Verify(
            s => s.GetDashboardKPIsAsync(companyId, null), 
            Times.Once);
    }

    [Fact]
    public async Task GetDashboardKPIs_ShouldReturnOkResult_WithDashboardKPIs()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var asOfDate = DateTime.UtcNow;
        var expectedKPIs = new DashboardKPIDto
        {
            CompanyId = companyId,
            AsOfDate = asOfDate,
            TotalRevenueMTD = 89000.00m,
            TotalRevenueYTD = 320000.00m,
            CashBalance = 150000.00m,
            EmployeeCount = 75,
            TotalPayrollCostMTD = 35000.00m,
            GeneratedAt = DateTime.UtcNow
        };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, asOfDate))
            .ReturnsAsync(expectedKPIs);

        // Act
        var result = await _controller.GetDashboardKPIs(companyId, asOfDate);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedKPIs);
        
        _mockDashboardService.Verify(
            s => s.GetDashboardKPIsAsync(companyId, asOfDate), 
            Times.Once);
    }

    [Fact]
    public async Task GetDashboardKPIs_ShouldCallService_WithNullAsOfDate_WhenNotProvided()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var expectedKPIs = new DashboardKPIDto
        {
            CompanyId = companyId,
            AsOfDate = DateTime.UtcNow,
            TotalRevenueMTD = 89000.00m,
            GeneratedAt = DateTime.UtcNow
        };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, null))
            .ReturnsAsync(expectedKPIs);

        // Act
        var result = await _controller.GetDashboardKPIs(companyId, null);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockDashboardService.Verify(
            s => s.GetDashboardKPIsAsync(companyId, null), 
            Times.Once);
    }

    [Fact]
    public async Task GetKPIs_ShouldLogInformation_WhenCalled()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var expectedKPIs = new DashboardKPIDto
        {
            CompanyId = companyId,
            GeneratedAt = DateTime.UtcNow
        };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, null))
            .ReturnsAsync(expectedKPIs);

        // Act
        await _controller.GetKPIs(companyId, null);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("KPI data requested")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetDashboardKPIs_ShouldLogInformation_WhenCalled()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var expectedKPIs = new DashboardKPIDto
        {
            CompanyId = companyId,
            GeneratedAt = DateTime.UtcNow
        };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, null))
            .ReturnsAsync(expectedKPIs);

        // Act
        await _controller.GetDashboardKPIs(companyId, null);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Dashboard KPI data requested")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
