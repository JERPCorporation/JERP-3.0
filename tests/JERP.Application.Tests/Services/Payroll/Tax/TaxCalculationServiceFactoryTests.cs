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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace JERP.Application.Tests.Services.Payroll.Tax;

public class TaxCalculationServiceFactoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly TaxCalculationServiceFactory _factory;

    public TaxCalculationServiceFactoryTests()
    {
        var services = new ServiceCollection();
        
        // Register in-memory database for testing
        services.AddDbContext<JerpDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}"));
        
        services.AddLogging();
        
        // Register tax services
        services.AddScoped<TaxCalculationService>();
        services.AddScoped<GuatemalaTaxCalculationService>();
        
        _serviceProvider = services.BuildServiceProvider();
        
        var logger = _serviceProvider.GetRequiredService<ILogger<TaxCalculationServiceFactory>>();
        _factory = new TaxCalculationServiceFactory(_serviceProvider, logger);
    }

    [Theory]
    [InlineData("GT")]
    [InlineData("GTM")]
    [InlineData("GUATEMALA")]
    [InlineData("gt")]
    [InlineData("gtm")]
    [InlineData("guatemala")]
    public void GetTaxService_WithGuatemalaCountryCode_ShouldReturnGuatemalaService(string countryCode)
    {
        // Act
        var service = _factory.GetTaxService(countryCode);

        // Assert
        service.Should().BeOfType<GuatemalaTaxCalculationService>();
    }

    [Theory]
    [InlineData("US")]
    [InlineData("USA")]
    [InlineData("UNITED STATES")]
    [InlineData("us")]
    [InlineData("usa")]
    public void GetTaxService_WithUSCountryCode_ShouldReturnUSService(string countryCode)
    {
        // Act
        var service = _factory.GetTaxService(countryCode);

        // Assert
        service.Should().BeOfType<TaxCalculationService>();
    }

    [Theory]
    [InlineData("MX")]
    [InlineData("CA")]
    [InlineData("UNKNOWN")]
    [InlineData("")]
    public void GetTaxService_WithUnknownCountryCode_ShouldReturnUSServiceAsDefault(string countryCode)
    {
        // Act
        var service = _factory.GetTaxService(countryCode);

        // Assert
        service.Should().BeOfType<TaxCalculationService>();
    }

    [Fact]
    public void GetTaxService_ShouldBeCaseInsensitive()
    {
        // Act
        var service1 = _factory.GetTaxService("GT");
        var service2 = _factory.GetTaxService("gt");
        var service3 = _factory.GetTaxService("Guatemala");

        // Assert
        service1.Should().BeOfType<GuatemalaTaxCalculationService>();
        service2.Should().BeOfType<GuatemalaTaxCalculationService>();
        service3.Should().BeOfType<GuatemalaTaxCalculationService>();
    }

    [Fact]
    public void GetTaxService_WithNullCountryCode_ShouldReturnUSServiceAsDefault()
    {
        // Act
        var service = _factory.GetTaxService(null!);

        // Assert
        service.Should().BeOfType<TaxCalculationService>();
    }
}
