using JERP.Api.Controllers;
using JERP.Application.Services.Reports;
using JERP.Application.DTOs.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace JERP.Api.Tests.Controllers;

public class KPIControllerTests
{
    private readonly Mock<ILogger<KPIController>> _mockLogger;
    private readonly Mock<IDashboardService> _mockDashboardService;
    private readonly KPIController _controller;

    public KPIControllerTests()
    {
        _mockLogger = new Mock<ILogger<KPIController>>();
        _mockDashboardService = new Mock<IDashboardService>();
        _controller = new KPIController(_mockLogger.Object, _mockDashboardService.Object);
    }

    [Fact]
    public async Task GetKPIs_ShouldReturnOkResult_WithDashboardKPIs()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var asOfDate = DateTime.UtcNow;
        var expectedKpis = new DashboardKPIDto
        {
            CompanyId = companyId,
            AsOfDate = asOfDate,
            TotalRevenueMTD = 150000.00m,
            TotalRevenueYTD = 1200000.00m,
            CashBalance = 50000.00m,
            EmployeeCount = 25,
            TotalPayrollCostMTD = 75000.00m,
            GeneratedAt = DateTime.UtcNow
        };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, asOfDate))
            .ReturnsAsync(expectedKpis);

        // Act
        var result = await _controller.GetKPIs(companyId, asOfDate);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedKpis);
    }

    [Fact]
    public async Task GetKPIs_ShouldCallDashboardService_WithCorrectParameters()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var asOfDate = DateTime.UtcNow;
        var kpis = new DashboardKPIDto { CompanyId = companyId };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, asOfDate))
            .ReturnsAsync(kpis);

        // Act
        await _controller.GetKPIs(companyId, asOfDate);

        // Assert
        _mockDashboardService.Verify(
            s => s.GetDashboardKPIsAsync(companyId, asOfDate),
            Times.Once);
    }

    [Fact]
    public async Task GetKPIs_ShouldUseNullAsOfDate_WhenNotProvided()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var kpis = new DashboardKPIDto { CompanyId = companyId };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, null))
            .ReturnsAsync(kpis);

        // Act
        await _controller.GetKPIs(companyId, null);

        // Assert
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
        var expectedKpis = new DashboardKPIDto
        {
            CompanyId = companyId,
            AsOfDate = asOfDate,
            TotalRevenueMTD = 45000.00m,
            TotalRevenueYTD = 540000.00m,
            CashBalance = 234500.00m,
            EmployeeCount = 12,
            TotalPayrollCostMTD = 25000.00m,
            InventoryValue = 234500.00m,
            GeneratedAt = DateTime.UtcNow
        };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, asOfDate))
            .ReturnsAsync(expectedKpis);

        // Act
        var result = await _controller.GetDashboardKPIs(companyId, asOfDate);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedKpis);
    }

    [Fact]
    public async Task GetDashboardKPIs_ShouldCallDashboardService_WithCorrectParameters()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var asOfDate = DateTime.UtcNow;
        var kpis = new DashboardKPIDto { CompanyId = companyId };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, asOfDate))
            .ReturnsAsync(kpis);

        // Act
        await _controller.GetDashboardKPIs(companyId, asOfDate);

        // Assert
        _mockDashboardService.Verify(
            s => s.GetDashboardKPIsAsync(companyId, asOfDate),
            Times.Once);
    }

    [Fact]
    public async Task GetDashboardKPIs_ShouldReturnRealValues_NotMockData()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var realKpis = new DashboardKPIDto
        {
            CompanyId = companyId,
            TotalRevenueMTD = 50000.00m,
            TotalRevenueYTD = 600000.00m,
            CashBalance = 100000.00m,
            EmployeeCount = 30,
            TotalPayrollCostMTD = 40000.00m
        };

        _mockDashboardService
            .Setup(s => s.GetDashboardKPIsAsync(companyId, null))
            .ReturnsAsync(realKpis);

        // Act
        var result = await _controller.GetDashboardKPIs(companyId, null);

        // Assert
        var okResult = result as OkObjectResult;
        var returnedKpis = okResult!.Value as DashboardKPIDto;
        
        returnedKpis.Should().NotBeNull();
        returnedKpis!.TotalRevenueMTD.Should().Be(50000.00m);
        returnedKpis.TotalRevenueYTD.Should().Be(600000.00m);
        returnedKpis.CashBalance.Should().Be(100000.00m);
        returnedKpis.EmployeeCount.Should().Be(30);
        returnedKpis.TotalPayrollCostMTD.Should().Be(40000.00m);
    }
}
