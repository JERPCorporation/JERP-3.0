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
using JERP.Application.DTOs.Compliance;
using JERP.Application.DTOs.Statistics;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class ComplianceViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<ComplianceViolationDto> _violations = new();

    [ObservableProperty]
    private ComplianceViolationDto? _selectedViolation;

    [ObservableProperty]
    private string _severityFilter = "All";

    [ObservableProperty]
    private string _statusFilter = "All";

    [ObservableProperty]
    private double _complianceScore;

    public ObservableCollection<string> SeverityOptions { get; } = new()
    {
        "All", "Critical", "High", "Medium", "Low"
    };

    public ObservableCollection<string> StatusOptions { get; } = new()
    {
        "All", "Active", "Resolved", "Acknowledged"
    };

    public ComplianceViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadViolationsAsync();
    }

    [RelayCommand]
    private async Task LoadViolationsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = "api/v1/compliance/violations/active?";
            
            if (SeverityFilter != "All")
            {
                query += $"severity={SeverityFilter}&";
            }

            if (StatusFilter != "All")
            {
                query += $"status={StatusFilter}&";
            }

            var violations = await _apiClient.GetAsync<List<ComplianceViolationDto>>(query.TrimEnd('&', '?'));
            
            if (violations != null)
            {
                Violations.Clear();
                foreach (var violation in violations)
                {
                    Violations.Add(violation);
                }
            }

            var scoreResponse = await _apiClient.GetAsync<dynamic>("api/v1/compliance/score");
            if (scoreResponse != null)
            {
                ComplianceScore = scoreResponse.Score ?? 0;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load violations: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ResolveViolationAsync(ComplianceViolationDto? violation)
    {
        if (violation == null) return;

        try
        {
            await _apiClient.PostAsync<object>($"api/v1/compliance/violations/{violation.Id}/resolve", new { });
            await LoadViolationsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to resolve violation: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ViewViolationDetails(ComplianceViolationDto? violation)
    {
        if (violation == null) return;
        SelectedViolation = violation;
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadViolationsAsync();
    }

    partial void OnSeverityFilterChanged(string value)
    {
        _ = LoadViolationsAsync();
    }

    partial void OnStatusFilterChanged(string value)
    {
        _ = LoadViolationsAsync();
    }
}
