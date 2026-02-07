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

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JERP.Application.DTOs.Compliance;
using JERP.Application.DTOs.Dashboard;
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
            var overviewTask = _apiClient.GetAsync<DashboardOverviewDto>("api/v1/dashboard/overview");
            var violationsTask = _apiClient.GetAsync<List<ComplianceViolationDto>>("api/v1/compliance/violations/active");

            await Task.WhenAll(overviewTask, violationsTask);

            var overview = await overviewTask;
            if (overview != null)
            {
                // Map overview data to dashboard properties
                ComplianceScore = 85; // Default score - can be enhanced later
                CriticalViolationCount = overview.CriticalViolations;
                ActiveEmployeeCount = overview.ActiveEmployees;
                PendingTimesheetCount = overview.PendingTimesheets;
                
                // For now, set other violation counts to 0 - can be enhanced later
                HighViolationCount = 0;
                MediumViolationCount = 0;
                LowViolationCount = 0;
            }

            var violations = await violationsTask;
            if (violations != null)
            {
                RecentViolations.Clear();
                foreach (var violation in violations.Take(10))
                {
                    RecentViolations.Add(violation);
                }
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
