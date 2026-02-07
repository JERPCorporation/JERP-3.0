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
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JERP.Application.DTOs.Payroll;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class PayrollViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<PayPeriodDto> _payPeriods = new();

    [ObservableProperty]
    private PayPeriodDto? _selectedPayPeriod;

    [ObservableProperty]
    private ObservableCollection<PayrollRecordDto> _payrollRecords = new();

    [ObservableProperty]
    private int _selectedYear = DateTime.Now.Year;

    [ObservableProperty]
    private decimal _totalGrossPay;

    [ObservableProperty]
    private decimal _totalNetPay;

    public PayrollViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadPayPeriodsAsync();
    }

    [RelayCommand]
    private async Task LoadPayPeriodsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var payPeriods = await _apiClient.GetAsync<List<PayPeriodDto>>($"api/v1/payroll/periods?year={SelectedYear}");
            
            if (payPeriods != null)
            {
                PayPeriods.Clear();
                foreach (var period in payPeriods)
                {
                    PayPeriods.Add(period);
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load pay periods: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void CreatePayPeriod()
    {
        // TODO: Open pay period dialog
    }

    [RelayCommand]
    private async Task ProcessPayrollAsync()
    {
        if (SelectedPayPeriod == null) return;

        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>($"api/v1/payroll/periods/{SelectedPayPeriod.Id}/process", new { });
            await ViewPayrollRecordsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to process payroll: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ApprovePayrollAsync()
    {
        if (SelectedPayPeriod == null) return;

        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>($"api/v1/payroll/periods/{SelectedPayPeriod.Id}/approve", new { });
            await LoadPayPeriodsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to approve payroll: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ViewPayrollRecordsAsync()
    {
        if (SelectedPayPeriod == null) return;

        try
        {
            IsBusy = true;
            var records = await _apiClient.GetAsync<List<PayrollRecordDto>>(
                $"api/v1/payroll/periods/{SelectedPayPeriod.Id}/records");
            
            if (records != null)
            {
                PayrollRecords.Clear();
                TotalGrossPay = 0;
                TotalNetPay = 0;

                foreach (var record in records)
                {
                    PayrollRecords.Add(record);
                    TotalGrossPay += record.GrossPay;
                    TotalNetPay += record.NetPay;
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load payroll records: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ViewPayStubAsync(PayrollRecordDto? record)
    {
        if (record == null) return;

        try
        {
            var pdfBytes = await _apiClient.GetBytesAsync($"api/v1/payroll/records/{record.Id}/paystub");
            
            var tempPath = Path.Combine(Path.GetTempPath(), $"PayStub_{record.Id}.pdf");
            await File.WriteAllBytesAsync(tempPath, pdfBytes);
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = tempPath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            SetError($"Failed to view pay stub: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task DownloadAllPayStubsAsync()
    {
        if (SelectedPayPeriod == null) return;

        try
        {
            IsBusy = true;
            var zipBytes = await _apiClient.GetBytesAsync($"api/v1/payroll/periods/{SelectedPayPeriod.Id}/paystubs/download");
            
            var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var filePath = Path.Combine(downloadsPath, "Downloads", $"PayStubs_{SelectedPayPeriod.Id}.zip");
            
            await File.WriteAllBytesAsync(filePath, zipBytes);
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Path.GetDirectoryName(filePath)!,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            SetError($"Failed to download pay stubs: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSelectedPayPeriodChanged(PayPeriodDto? value)
    {
        if (value != null)
        {
            _ = ViewPayrollRecordsAsync();
        }
    }

    partial void OnSelectedYearChanged(int value)
    {
        _ = LoadPayPeriodsAsync();
    }
}
