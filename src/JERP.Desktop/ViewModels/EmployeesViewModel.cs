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
using JERP.Application.DTOs.Employees;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class EmployeesViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<EmployeeDto> _employees = new();

    [ObservableProperty]
    private EmployeeDto? _selectedEmployee;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private int _totalPages = 1;

    [ObservableProperty]
    private int _pageSize = 25;

    public EmployeesViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadEmployeesAsync();
    }

    [RelayCommand]
    private async Task LoadEmployeesAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/v1/employees?page={CurrentPage}&pageSize={PageSize}";
            
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query += $"&search={Uri.EscapeDataString(SearchText)}";
            }

            var response = await _apiClient.GetAsync<dynamic>(query);
            
            if (response != null)
            {
                Employees.Clear();
                
                var items = response.items as IEnumerable<dynamic>;
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        Employees.Add(new EmployeeDto
                        {
                            Id = item.id,
                            EmployeeNumber = item.employeeNumber,
                            FirstName = item.firstName,
                            LastName = item.lastName,
                            Email = item.email,
                            DepartmentId = item.departmentId,
                            Status = item.status,
                            HireDate = item.hireDate,
                            HourlyRate = item.hourlyRate,
                            CompanyId = item.companyId,
                            EmploymentType = item.employmentType,
                            Classification = item.classification,
                            PayFrequency = item.payFrequency,
                            CreatedAt = item.createdAt,
                            UpdatedAt = item.updatedAt
                        });
                    }
                }
                
                TotalPages = response.totalPages ?? 1;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load employees: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        CurrentPage = 1;
        await LoadEmployeesAsync();
    }

    [RelayCommand]
    private void AddEmployee()
    {
        // TODO: Open employee dialog
    }

    [RelayCommand]
    private void EditEmployee(EmployeeDto? employee)
    {
        if (employee == null) return;
        // TODO: Open employee dialog
    }

    [RelayCommand]
    private async Task DeleteEmployeeAsync(EmployeeDto? employee)
    {
        if (employee == null) return;

        try
        {
            await _apiClient.DeleteAsync($"api/v1/employees/{employee.Id}");
            await LoadEmployeesAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to delete employee: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            await LoadEmployeesAsync();
        }
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadEmployeesAsync();
        }
    }
}
