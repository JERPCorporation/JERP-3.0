/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JERP.Application.DTOs.Timesheets;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class TimesheetsViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<TimesheetDto> _timesheets = new();

    [ObservableProperty]
    private TimesheetDto? _selectedTimesheet;

    [ObservableProperty]
    private int? _selectedEmployeeId;

    [ObservableProperty]
    private DateTime _startDate = DateTime.Today.AddDays(-30);

    [ObservableProperty]
    private DateTime _endDate = DateTime.Today;

    [ObservableProperty]
    private string _statusFilter = "All";

    public ObservableCollection<string> StatusOptions { get; } = new()
    {
        "All", "Draft", "Submitted", "Approved", "Rejected"
    };

    public TimesheetsViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadTimesheetsAsync();
    }

    [RelayCommand]
    private async Task LoadTimesheetsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/v1/timesheets?startDate={StartDate:yyyy-MM-dd}&endDate={EndDate:yyyy-MM-dd}";
            
            if (SelectedEmployeeId.HasValue)
            {
                query += $"&employeeId={SelectedEmployeeId.Value}";
            }

            if (StatusFilter != "All")
            {
                query += $"&status={StatusFilter}";
            }

            var timesheets = await _apiClient.GetAsync<List<TimesheetDto>>(query);
            
            if (timesheets != null)
            {
                Timesheets.Clear();
                foreach (var timesheet in timesheets)
                {
                    Timesheets.Add(timesheet);
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load timesheets: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void AddTimesheet()
    {
        // TODO: Open timesheet dialog
    }

    [RelayCommand]
    private void EditTimesheet(TimesheetDto? timesheet)
    {
        if (timesheet == null) return;
        // TODO: Open timesheet dialog
    }

    [RelayCommand]
    private async Task SubmitTimesheetAsync(TimesheetDto? timesheet)
    {
        if (timesheet == null) return;

        try
        {
            await _apiClient.PostAsync<object>($"api/v1/timesheets/{timesheet.Id}/submit", new { });
            await LoadTimesheetsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to submit timesheet: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task ApproveTimesheetAsync(TimesheetDto? timesheet)
    {
        if (timesheet == null) return;

        try
        {
            await _apiClient.PostAsync<object>($"api/v1/timesheets/{timesheet.Id}/approve", new { });
            await LoadTimesheetsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to approve timesheet: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task DeleteTimesheetAsync(TimesheetDto? timesheet)
    {
        if (timesheet == null) return;

        try
        {
            await _apiClient.DeleteAsync($"api/v1/timesheets/{timesheet.Id}");
            await LoadTimesheetsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to delete timesheet: {ex.Message}");
        }
    }
}
