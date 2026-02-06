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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class FinanceViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private string _activeFinanceTab = "ChartOfAccounts";

    [ObservableProperty]
    private int _totalAccountsCount;

    [ObservableProperty]
    private int _activeAccountsCount;

    [ObservableProperty]
    private int _totalVendorsCount;

    [ObservableProperty]
    private int _cannabisVendorsCount;

    [ObservableProperty]
    private int _totalCustomersCount;

    [ObservableProperty]
    private int _cannabisCustomersCount;

    [ObservableProperty]
    private decimal _outstandingReceivablesTotal;

    [ObservableProperty]
    private decimal _outstandingPayablesTotal;

    [ObservableProperty]
    private int _unbilledInvoicesCount;

    [ObservableProperty]
    private int _unpaidBillsCount;

    [ObservableProperty]
    private decimal _cashBalanceSnapshot;

    [ObservableProperty]
    private decimal _grossRevenueMonthToDate;

    public FinanceViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadFinanceDashboardMetricsAsync();
    }

    [RelayCommand]
    private async Task LoadFinanceDashboardMetricsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var metrics = await _apiClient.GetAsync<dynamic>("api/v1/finance/dashboard/metrics");
            
            if (metrics != null)
            {
                TotalAccountsCount = metrics.totalAccountsCount ?? 0;
                ActiveAccountsCount = metrics.activeAccountsCount ?? 0;
                TotalVendorsCount = metrics.totalVendorsCount ?? 0;
                CannabisVendorsCount = metrics.cannabisVendorsCount ?? 0;
                TotalCustomersCount = metrics.totalCustomersCount ?? 0;
                CannabisCustomersCount = metrics.cannabisCustomersCount ?? 0;
                OutstandingReceivablesTotal = metrics.outstandingReceivablesTotal ?? 0m;
                OutstandingPayablesTotal = metrics.outstandingPayablesTotal ?? 0m;
                UnbilledInvoicesCount = metrics.unbilledInvoicesCount ?? 0;
                UnpaidBillsCount = metrics.unpaidBillsCount ?? 0;
                CashBalanceSnapshot = metrics.cashBalanceSnapshot ?? 0m;
                GrossRevenueMonthToDate = metrics.grossRevenueMonthToDate ?? 0m;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load finance dashboard metrics: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void NavigateToChartOfAccounts()
    {
        ActiveFinanceTab = "ChartOfAccounts";
    }

    [RelayCommand]
    private void NavigateToJournalEntries()
    {
        ActiveFinanceTab = "JournalEntries";
    }

    [RelayCommand]
    private void NavigateToVendors()
    {
        ActiveFinanceTab = "Vendors";
    }

    [RelayCommand]
    private void NavigateToBills()
    {
        ActiveFinanceTab = "Bills";
    }

    [RelayCommand]
    private void NavigateToCustomers()
    {
        ActiveFinanceTab = "Customers";
    }

    [RelayCommand]
    private void NavigateToInvoices()
    {
        ActiveFinanceTab = "Invoices";
    }

    [RelayCommand]
    private void NavigateToFinancialReports()
    {
        ActiveFinanceTab = "FinancialReports";
    }

    partial void OnActiveFinanceTabChanged(string value)
    {
        _ = LoadFinanceDashboardMetricsAsync();
    }
}
