/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

using FluentAssertions;
using JERP.Application.Services.Payroll.Tax;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JERP.Application.Tests.Services.Payroll.Tax;

public class GuatemalaTaxCalculationServiceTests
{
    private readonly JerpDbContext _context;
    private readonly Mock<ILogger<GuatemalaTaxCalculationService>> _mockLogger;
    private readonly GuatemalaTaxCalculationService _service;

    public GuatemalaTaxCalculationServiceTests()
    {
        var options = new DbContextOptionsBuilder<JerpDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        
        _context = new JerpDbContext(options);
        _mockLogger = new Mock<ILogger<GuatemalaTaxCalculationService>>();
        _service = new GuatemalaTaxCalculationService(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task CalculateTaxesAsync_WithLowIncome_ShouldReturnZeroISR()
    {
        // Arrange - Q3,000 monthly (Q36,000 annually) - below exempt threshold of Q48,000
        var request = new TaxCalculationRequest
        {
            EmployeeId = Guid.NewGuid(),
            GrossPay = 3000m,
            PayPeriods = 12,
            YTDGrossPay = 27000m
        };

        // Act
        var result = await _service.CalculateTaxesAsync(request);

        // Assert
        result.FederalTax.Should().Be(0); // ISR should be 0 (below exempt threshold)
        result.SocialSecurityTax.Should().Be(144.90m); // 3000 * 4.83% = 144.90
        result.StateTax.Should().Be(0);
        result.MedicareTax.Should().Be(0);
        result.TotalTaxes.Should().Be(144.90m);
    }

    [Fact]
    public async Task CalculateTaxesAsync_WithModerateIncome_ShouldCalculateISRCorrectly()
    {
        // Arrange - Q6,000 monthly (Q72,000 annually)
        // Taxable: 72,000 - 48,000 (exempt) - 12,000 (deduction) = 12,000
        // ISR: 12,000 * 5% = 600 annually, 50 per month
        var request = new TaxCalculationRequest
        {
            EmployeeId = Guid.NewGuid(),
            GrossPay = 6000m,
            PayPeriods = 12,
            YTDGrossPay = 54000m
        };

        // Act
        var result = await _service.CalculateTaxesAsync(request);

        // Assert
        result.FederalTax.Should().Be(50.00m); // 600 / 12 = 50
        result.SocialSecurityTax.Should().Be(289.80m); // 6000 * 4.83% = 289.80
        result.StateTax.Should().Be(0);
        result.MedicareTax.Should().Be(0);
        result.TotalTaxes.Should().Be(339.80m);
    }

    [Fact]
    public async Task CalculateTaxesAsync_WithHighIncome_ShouldApplySecondBracket()
    {
        // Arrange - Q35,000 monthly (Q420,000 annually)
        // Taxable: 420,000 - 48,000 - 12,000 = 360,000
        // ISR: 15,000 + ((360,000 - 300,000) * 7%) = 15,000 + 4,200 = 19,200 annually
        // Per month: 19,200 / 12 = 1,600
        var request = new TaxCalculationRequest
        {
            EmployeeId = Guid.NewGuid(),
            GrossPay = 35000m,
            PayPeriods = 12,
            YTDGrossPay = 315000m
        };

        // Act
        var result = await _service.CalculateTaxesAsync(request);

        // Assert
        result.FederalTax.Should().Be(1600.00m); // 19,200 / 12 = 1,600
        result.SocialSecurityTax.Should().Be(1690.50m); // 35,000 * 4.83% = 1,690.50
        result.StateTax.Should().Be(0);
        result.MedicareTax.Should().Be(0);
        result.TotalTaxes.Should().Be(3290.50m);
    }

    [Fact]
    public async Task CalculateTaxesAsync_WithFirstBracketMaxIncome_ShouldCalculateCorrectly()
    {
        // Arrange - Income exactly at first bracket limit
        // Q30,000 monthly (Q360,000 annually)
        // Taxable: 360,000 - 48,000 - 12,000 = 300,000 (exactly at limit)
        // ISR: 300,000 * 5% = 15,000 annually, 1,250 per month
        var request = new TaxCalculationRequest
        {
            EmployeeId = Guid.NewGuid(),
            GrossPay = 30000m,
            PayPeriods = 12,
            YTDGrossPay = 270000m
        };

        // Act
        var result = await _service.CalculateTaxesAsync(request);

        // Assert
        result.FederalTax.Should().Be(1250.00m); // 15,000 / 12 = 1,250
        result.SocialSecurityTax.Should().Be(1449.00m); // 30,000 * 4.83% = 1,449
        result.StateTax.Should().Be(0);
        result.MedicareTax.Should().Be(0);
    }

    [Fact]
    public async Task CalculateTaxesAsync_BiweeklyPayPeriod_ShouldCalculateCorrectly()
    {
        // Arrange - Q3,000 biweekly (Q78,000 annually with 26 pay periods)
        // Taxable: 78,000 - 48,000 - 12,000 = 18,000
        // ISR: 18,000 * 5% = 900 annually, 900 / 26 = 34.62 per period
        var request = new TaxCalculationRequest
        {
            EmployeeId = Guid.NewGuid(),
            GrossPay = 3000m,
            PayPeriods = 26,
            YTDGrossPay = 72000m
        };

        // Act
        var result = await _service.CalculateTaxesAsync(request);

        // Assert
        result.FederalTax.Should().Be(34.62m); // 900 / 26 = 34.62
        result.SocialSecurityTax.Should().Be(144.90m); // 3000 * 4.83% = 144.90
    }

    [Fact]
    public async Task CalculateTaxesAsync_IGSSCalculation_ShouldBeAccurate()
    {
        // Arrange - Test IGSS calculation specifically
        var request = new TaxCalculationRequest
        {
            EmployeeId = Guid.NewGuid(),
            GrossPay = 5000m,
            PayPeriods = 12,
            YTDGrossPay = 45000m
        };

        // Act
        var result = await _service.CalculateTaxesAsync(request);

        // Assert - IGSS is 4.83% of gross pay
        result.SocialSecurityTax.Should().Be(241.50m); // 5000 * 0.0483 = 241.50
    }

    [Fact]
    public async Task CalculateTaxesAsync_ShouldReturnZeroForStateTaxAndMedicareTax()
    {
        // Arrange
        var request = new TaxCalculationRequest
        {
            EmployeeId = Guid.NewGuid(),
            GrossPay = 10000m,
            PayPeriods = 12,
            YTDGrossPay = 90000m
        };

        // Act
        var result = await _service.CalculateTaxesAsync(request);

        // Assert - Guatemala has no state tax or Medicare tax
        result.StateTax.Should().Be(0);
        result.MedicareTax.Should().Be(0);
    }
}
