/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JERP.Application.DTOs.Compliance;
using JERP.Application.DTOs.Statistics;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private double _complianceScore;

    [ObservableProperty]
    private int _criticalViolationCount;

    [ObservableProperty]
    private int _highViolationCount;

    [ObservableProperty]
    private int _mediumViolationCount;

    [ObservableProperty]
    private int _lowViolationCount;

    [ObservableProperty]
    private int _activeEmployeeCount;

    [ObservableProperty]
    private int _pendingTimesheetCount;

    [ObservableProperty]
    private ObservableCollection<ComplianceViolationDto> _recentViolations = new();

    public DashboardViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadDataAsync();
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        
        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var statsTask = _apiClient.GetAsync<ComplianceStatsDto>("api/compliance/stats");
            var violationsTask = _apiClient.GetAsync<List<ComplianceViolationDto>>("api/compliance/violations/recent?count=10");
            var employeesTask = _apiClient.GetAsync<EmployeeStatsDto>("api/employees/stats");
            var timesheetsTask = _apiClient.GetAsync<TimesheetStatsDto>("api/timesheets/stats");

            await Task.WhenAll(statsTask, violationsTask, employeesTask, timesheetsTask);

            var stats = await statsTask;
            if (stats != null)
            {
                ComplianceScore = stats.ComplianceScore;
                CriticalViolationCount = stats.CriticalCount;
                HighViolationCount = stats.HighCount;
                MediumViolationCount = stats.MediumCount;
                LowViolationCount = stats.LowCount;
            }

            var violations = await violationsTask;
            if (violations != null)
            {
                RecentViolations.Clear();
                foreach (var violation in violations)
                {
                    RecentViolations.Add(violation);
                }
            }

            var employeeStats = await employeesTask;
            if (employeeStats != null)
            {
                ActiveEmployeeCount = employeeStats.ActiveCount;
            }

            var timesheetStats = await timesheetsTask;
            if (timesheetStats != null)
            {
                PendingTimesheetCount = timesheetStats.PendingCount;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load dashboard data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDataAsync();
    }
}
