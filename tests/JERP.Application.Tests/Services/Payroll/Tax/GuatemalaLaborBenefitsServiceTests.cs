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
using Xunit;

namespace JERP.Application.Tests.Services.Payroll.Tax;

public class GuatemalaLaborBenefitsServiceTests
{
    private readonly GuatemalaLaborBenefitsService _service;

    public GuatemalaLaborBenefitsServiceTests()
    {
        _service = new GuatemalaLaborBenefitsService();
    }

    [Fact]
    public void CalculateMonthlyProvisions_ShouldCalculateAllProvisionsCorrectly()
    {
        // Arrange
        var monthlySalary = 5000m;

        // Act
        var result = _service.CalculateMonthlyProvisions(monthlySalary);

        // Assert
        result.Bono14Provision.Should().Be(416.67m); // 5000 / 12 = 416.67
        result.AguinaldoProvision.Should().Be(416.67m); // 5000 / 12 = 416.67
        result.IndemnizacionProvision.Should().Be(416.67m); // 5000 / 12 = 416.67
        result.VacacionesProvision.Should().Be(205.48m); // 5000 * (15/365) = 205.48
        result.IGSSPatronalCost.Should().Be(633.50m); // 5000 * 0.1267 = 633.50
        result.TotalMonthlyProvisions.Should().Be(2088.99m); // Sum of all
    }

    [Fact]
    public void CalculateBono14_WithFullYear_ShouldReturnFullSalary()
    {
        // Arrange
        var monthlySalary = 6000m;
        var monthsWorked = 12;

        // Act
        var result = _service.CalculateBono14(monthlySalary, monthsWorked);

        // Assert
        result.Should().Be(6000m); // Full month salary
    }

    [Fact]
    public void CalculateBono14_WithSixMonths_ShouldReturnHalfSalary()
    {
        // Arrange
        var monthlySalary = 6000m;
        var monthsWorked = 6;

        // Act
        var result = _service.CalculateBono14(monthlySalary, monthsWorked);

        // Assert
        result.Should().Be(3000m); // Half month salary (6000 * 6 / 12)
    }

    [Fact]
    public void CalculateBono14_WithMoreThan12Months_ShouldCapAt12()
    {
        // Arrange
        var monthlySalary = 6000m;
        var monthsWorked = 15;

        // Act
        var result = _service.CalculateBono14(monthlySalary, monthsWorked);

        // Assert
        result.Should().Be(6000m); // Should cap at 12 months
    }

    [Fact]
    public void CalculateAguinaldo_WithFullYear_ShouldReturnFullSalary()
    {
        // Arrange
        var monthlySalary = 7000m;
        var monthsWorked = 12;

        // Act
        var result = _service.CalculateAguinaldo(monthlySalary, monthsWorked);

        // Assert
        result.Should().Be(7000m); // Full month salary
    }

    [Fact]
    public void CalculateAguinaldo_WithThreeMonths_ShouldReturnProportional()
    {
        // Arrange
        var monthlySalary = 8000m;
        var monthsWorked = 3;

        // Act
        var result = _service.CalculateAguinaldo(monthlySalary, monthsWorked);

        // Assert
        result.Should().Be(2000m); // 8000 * 3 / 12 = 2000
    }

    [Fact]
    public void CalculateIndemnizacion_WithOneYear_ShouldReturnFullSalary()
    {
        // Arrange
        var monthlySalary = 5000m;
        var totalMonthsWorked = 12;

        // Act
        var result = _service.CalculateIndemnizacion(monthlySalary, totalMonthsWorked);

        // Assert
        result.Should().Be(5000m); // 1 month salary for 1 year
    }

    [Fact]
    public void CalculateIndemnizacion_WithThreeYears_ShouldReturnThreeMonths()
    {
        // Arrange
        var monthlySalary = 6000m;
        var totalMonthsWorked = 36;

        // Act
        var result = _service.CalculateIndemnizacion(monthlySalary, totalMonthsWorked);

        // Assert
        result.Should().Be(18000m); // 3 months salary (6000 * 36 / 12)
    }

    [Fact]
    public void CalculateIndemnizacion_WithSixMonths_ShouldReturnProportional()
    {
        // Arrange
        var monthlySalary = 8000m;
        var totalMonthsWorked = 6;

        // Act
        var result = _service.CalculateIndemnizacion(monthlySalary, totalMonthsWorked);

        // Assert
        result.Should().Be(4000m); // Half month salary (8000 * 6 / 12)
    }

    [Fact]
    public void CalculateTotalEmployerMonthlyCost_ShouldIncludeAllCosts()
    {
        // Arrange
        var monthlySalary = 10000m;

        // Act
        var totalCost = _service.CalculateTotalEmployerMonthlyCost(monthlySalary);

        // Assert
        // Salary: 10000
        // IGSS Patronal: 1267 (10000 * 0.1267)
        // Bono 14: 833.33 (10000 / 12)
        // Aguinaldo: 833.33 (10000 / 12)
        // Indemnización: 833.33 (10000 / 12)
        // Vacaciones: 410.96 (10000 * 15/365)
        // Total: ~14177.95
        totalCost.Should().BeApproximately(14177.95m, 0.01m);
    }

    [Fact]
    public void CalculateMonthlyProvisions_IGSSPatronalCost_ShouldBe1267Percent()
    {
        // Arrange
        var monthlySalary = 10000m;

        // Act
        var result = _service.CalculateMonthlyProvisions(monthlySalary);

        // Assert
        result.IGSSPatronalCost.Should().Be(1267.00m); // 10000 * 0.1267 = 1267
    }

    [Theory]
    [InlineData(3000, 250.00, 250.00, 250.00)] // Small salary
    [InlineData(5000, 416.67, 416.67, 416.67)] // Medium salary
    [InlineData(10000, 833.33, 833.33, 833.33)] // Large salary
    public void CalculateMonthlyProvisions_ShouldCalculateProvisionsCorrectly(
        decimal salary, decimal expectedBono14, decimal expectedAguinaldo, decimal expectedIndemnizacion)
    {
        // Act
        var result = _service.CalculateMonthlyProvisions(salary);

        // Assert
        result.Bono14Provision.Should().Be(expectedBono14);
        result.AguinaldoProvision.Should().Be(expectedAguinaldo);
        result.IndemnizacionProvision.Should().Be(expectedIndemnizacion);
    }
}
