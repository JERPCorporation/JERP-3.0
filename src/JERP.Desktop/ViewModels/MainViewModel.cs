using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JERP.Application.DTOs;
using JERP.Desktop.Services;
using JERP.Desktop.Views;

namespace JERP.Desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private UserDto? _currentUser;

    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private string _selectedMenuItem = "Dashboard";

    public MainViewModel(
        IAuthenticationService authService,
        IServiceProvider serviceProvider)
    {
        _authService = authService;
        _serviceProvider = serviceProvider;
        
        CurrentUser = _authService.CurrentUser;
        
        ShowDashboard();
    }

    [RelayCommand]
    private void ShowDashboard()
    {
        SelectedMenuItem = "Dashboard";
        var dashboardView = _serviceProvider.GetService(typeof(DashboardView)) as DashboardView;
        CurrentView = dashboardView;
    }

    [RelayCommand]
    private void ShowEmployees()
    {
        SelectedMenuItem = "Employees";
        var employeesView = _serviceProvider.GetService(typeof(EmployeesView)) as EmployeesView;
        CurrentView = employeesView;
    }

    [RelayCommand]
    private void ShowTimesheets()
    {
        SelectedMenuItem = "Timesheets";
        var timesheetsView = _serviceProvider.GetService(typeof(TimesheetsView)) as TimesheetsView;
        CurrentView = timesheetsView;
    }

    [RelayCommand]
    private void ShowPayroll()
    {
        SelectedMenuItem = "Payroll";
        var payrollView = _serviceProvider.GetService(typeof(PayrollView)) as PayrollView;
        CurrentView = payrollView;
    }

    [RelayCommand]
    private void ShowCompliance()
    {
        SelectedMenuItem = "Compliance";
        var complianceView = _serviceProvider.GetService(typeof(ComplianceView)) as ComplianceView;
        CurrentView = complianceView;
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        
        var loginWindow = _serviceProvider.GetService(typeof(LoginWindow)) as LoginWindow;
        loginWindow?.Show();
        
        Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
    }
}
