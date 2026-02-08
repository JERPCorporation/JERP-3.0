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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JERP.Application.Services.Payroll.Tax;

/// <summary>
/// Factory that resolves the appropriate ITaxCalculationService
/// based on company country configuration.
/// </summary>
public interface ITaxCalculationServiceFactory
{
    ITaxCalculationService GetTaxService(string countryCode);
}

public class TaxCalculationServiceFactory : ITaxCalculationServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TaxCalculationServiceFactory> _logger;

    public TaxCalculationServiceFactory(
        IServiceProvider serviceProvider,
        ILogger<TaxCalculationServiceFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public ITaxCalculationService GetTaxService(string countryCode)
    {
        _logger.LogInformation("Resolving tax service for country: {CountryCode}", countryCode);

        return countryCode?.ToUpperInvariant() switch
        {
            "GT" or "GTM" or "GUATEMALA" => _serviceProvider.GetRequiredService<GuatemalaTaxCalculationService>(),
            "US" or "USA" or "UNITED STATES" => _serviceProvider.GetRequiredService<TaxCalculationService>(),
            _ => _serviceProvider.GetRequiredService<TaxCalculationService>() // Default to US
        };
    }
}
