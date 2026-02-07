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

using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JERP.Desktop.Services;
using JERP.Desktop.ViewModels;
using JERP.Desktop.ViewModels.Finance;
using JERP.Desktop.Views;
using JERP.Desktop.Views.Finance;

namespace JERP.Desktop;

public partial class App : System.Windows.Application
{
    public IServiceProvider ServiceProvider { get; private set; } = null!;
    public IConfiguration Configuration { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);

        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
        loginWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(Configuration);
        
        services.AddSingleton<IRegistryService, RegistryService>();
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<IApiClient, ApiClient>();

        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            var baseUrl = Configuration["Api:BaseUrl"] ?? "http://localhost:5000";
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(
                int.Parse(Configuration["Api:TimeoutSeconds"] ?? "30"));
        });

        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<EmployeesViewModel>();
        services.AddTransient<TimesheetsViewModel>();
        services.AddTransient<PayrollViewModel>();
        services.AddTransient<ComplianceViewModel>();
        
        // Finance ViewModels
        services.AddTransient<ViewModels.Finance.FinanceViewModel>();
        services.AddTransient<ViewModels.Finance.ChartOfAccountsViewModel>();
        services.AddTransient<ViewModels.Finance.JournalEntriesViewModel>();
        services.AddTransient<ViewModels.Finance.VendorsViewModel>();
        services.AddTransient<ViewModels.Finance.BillsViewModel>();
        services.AddTransient<ViewModels.Finance.CustomersViewModel>();
        services.AddTransient<ViewModels.Finance.InvoicesViewModel>();

        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();
        services.AddTransient<DashboardView>();
        services.AddTransient<EmployeesView>();
        services.AddTransient<TimesheetsView>();
        services.AddTransient<PayrollView>();
        services.AddTransient<ComplianceView>();
        
        // Finance Views
        services.AddTransient<FinanceView>();
        services.AddTransient<ChartOfAccountsView>();
        services.AddTransient<JournalEntriesView>();
        services.AddTransient<VendorsView>();
        services.AddTransient<BillsView>();
        services.AddTransient<CustomersView>();
        services.AddTransient<InvoicesView>();
    }
}
