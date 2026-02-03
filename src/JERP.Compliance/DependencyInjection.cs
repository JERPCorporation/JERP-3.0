using JERP.Compliance.Financial.Rules;
using JERP.Compliance.LaborLaw.Rules;
using JERP.Compliance.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JERP.Compliance;

/// <summary>
/// Extension methods for registering compliance services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all compliance services and rules with the service collection
    /// </summary>
    public static IServiceCollection AddComplianceServices(this IServiceCollection services)
    {
        // Register the compliance engine
        services.AddScoped<IComplianceEngine, ComplianceEngine>();

        // Register labor law rules
        services.AddScoped<CaliforniaDailyOvertimeRule>();
        services.AddScoped<CaliforniaSeventhDayOvertimeRule>();
        services.AddScoped<FederalWeeklyOvertimeRule>();
        services.AddScoped<MealBreakRule>();
        services.AddScoped<RestBreakRule>();
        services.AddScoped<MinimumWageRule>();
        services.AddScoped<ChildLaborRule>();

        // Register financial rules
        services.AddScoped<GAAPBalanceSheetRule>();
        services.AddScoped<GAAPRevenueRecognitionRule>();
        services.AddScoped<IFRSInventoryValuationRule>();

        return services;
    }
}
