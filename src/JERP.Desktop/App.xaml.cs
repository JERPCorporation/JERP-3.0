using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JERP.Desktop.Services;
using JERP.Desktop.ViewModels;
using JERP.Desktop.Views;

namespace JERP.Desktop;

public partial class App : Application
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

        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();
        services.AddTransient<DashboardView>();
        services.AddTransient<EmployeesView>();
        services.AddTransient<TimesheetsView>();
        services.AddTransient<PayrollView>();
        services.AddTransient<ComplianceView>();
    }
}
