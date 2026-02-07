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
using JERP.Application.DTOs.Finance;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class FinancialReportsViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private string _activeReportView = "ProfitAndLoss";

    [ObservableProperty]
    private DateTime _reportPeriodStartDate = new DateTime(DateTime.Now.Year, 1, 1);

    [ObservableProperty]
    private DateTime _reportPeriodEndDate = DateTime.Now;

    [ObservableProperty]
    private DateTime _balanceSheetAsOfDate = DateTime.Now;

    [ObservableProperty]
    private ProfitAndLossReportDto? _profitLossStatement;

    [ObservableProperty]
    private BalanceSheetReportDto? _balanceSheetStatement;

    [ObservableProperty]
    private ObservableCollection<AccountSummaryDto> _revenueLineItems = new();

    [ObservableProperty]
    private ObservableCollection<AccountSummaryDto> _cogsLineItems = new();

    [ObservableProperty]
    private ObservableCollection<AccountSummaryDto> _expenseLineItems = new();

    [ObservableProperty]
    private ObservableCollection<AccountSummaryDto> _assetLineItems = new();

    [ObservableProperty]
    private ObservableCollection<AccountSummaryDto> _liabilityLineItems = new();

    [ObservableProperty]
    private ObservableCollection<AccountSummaryDto> _equityLineItems = new();

    [ObservableProperty]
    private decimal _totalRevenueReported;

    [ObservableProperty]
    private decimal _totalCogsReported;

    [ObservableProperty]
    private decimal _grossProfitMargin;

    [ObservableProperty]
    private decimal _totalExpensesReported;

    [ObservableProperty]
    private decimal _netIncomeCalculated;

    [ObservableProperty]
    private decimal _total280EDeductibleExpenses;

    [ObservableProperty]
    private decimal _total280ENonDeductibleExpenses;

    [ObservableProperty]
    private decimal _totalAssetsReported;

    [ObservableProperty]
    private decimal _totalLiabilitiesReported;

    [ObservableProperty]
    private decimal _totalEquityReported;

    [ObservableProperty]
    private decimal _balanceSheetVariance;

    [ObservableProperty]
    private bool _isBalanceSheetBalanced;

    public FinancialReportsViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = GenerateProfitAndLossReportAsync();
    }

    [RelayCommand]
    private async Task GenerateProfitAndLossReportAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/finance/reports/profit-and-loss?startDate={ReportPeriodStartDate:yyyy-MM-dd}&endDate={ReportPeriodEndDate:yyyy-MM-dd}";
            
            var plReport = await _apiClient.GetAsync<ProfitAndLossReportDto>(query);
            
            if (plReport != null)
            {
                ProfitLossStatement = plReport;
                
                RevenueLineItems.Clear();
                CogsLineItems.Clear();
                ExpenseLineItems.Clear();
                
                TotalRevenueReported = 0;
                TotalCogsReported = 0;
                TotalExpensesReported = 0;
                Total280EDeductibleExpenses = 0;
                Total280ENonDeductibleExpenses = 0;

                foreach (var revenue in plReport.Revenue)
                {
                    RevenueLineItems.Add(revenue);
                    TotalRevenueReported += revenue.Balance;
                }

                foreach (var cogs in plReport.CostOfGoodsSold)
                {
                    CogsLineItems.Add(cogs);
                    TotalCogsReported += cogs.Balance;
                }

                GrossProfitMargin = TotalRevenueReported - TotalCogsReported;

                foreach (var expense in plReport.Expenses)
                {
                    ExpenseLineItems.Add(expense);
                    TotalExpensesReported += expense.Balance;

                    if (expense.IsNonDeductible)
                    {
                        Total280ENonDeductibleExpenses += expense.Balance;
                    }
                    else
                    {
                        Total280EDeductibleExpenses += expense.Balance;
                    }
                }

                NetIncomeCalculated = GrossProfitMargin - TotalExpensesReported;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to generate P&L report: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GenerateBalanceSheetReportAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/finance/reports/balance-sheet?asOfDate={BalanceSheetAsOfDate:yyyy-MM-dd}";
            
            var bsReport = await _apiClient.GetAsync<BalanceSheetReportDto>(query);
            
            if (bsReport != null)
            {
                BalanceSheetStatement = bsReport;
                
                AssetLineItems.Clear();
                LiabilityLineItems.Clear();
                EquityLineItems.Clear();
                
                TotalAssetsReported = 0;
                TotalLiabilitiesReported = 0;
                TotalEquityReported = 0;

                foreach (var asset in bsReport.Assets)
                {
                    AssetLineItems.Add(asset);
                    TotalAssetsReported += asset.Balance;
                }

                foreach (var liability in bsReport.Liabilities)
                {
                    LiabilityLineItems.Add(liability);
                    TotalLiabilitiesReported += liability.Balance;
                }

                foreach (var equity in bsReport.Equity)
                {
                    EquityLineItems.Add(equity);
                    TotalEquityReported += equity.Balance;
                }

                BalanceSheetVariance = TotalAssetsReported - (TotalLiabilitiesReported + TotalEquityReported);
                IsBalanceSheetBalanced = Math.Abs(BalanceSheetVariance) < 0.01m;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to generate balance sheet: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void SwitchToProfitAndLossView()
    {
        ActiveReportView = "ProfitAndLoss";
        _ = GenerateProfitAndLossReportAsync();
    }

    [RelayCommand]
    private void SwitchToBalanceSheetView()
    {
        ActiveReportView = "BalanceSheet";
        _ = GenerateBalanceSheetReportAsync();
    }

    [RelayCommand]
    private async Task ExportProfitLossStatementAsync()
    {
        try
        {
            IsBusy = true;
            var pdfBytes = await _apiClient.GetBytesAsync(
                $"api/finance/reports/profit-and-loss/export?startDate={ReportPeriodStartDate:yyyy-MM-dd}&endDate={ReportPeriodEndDate:yyyy-MM-dd}");
            
            var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var downloadsFolder = Path.Combine(downloadsPath, "Downloads");
            
            if (!Directory.Exists(downloadsFolder))
            {
                downloadsFolder = downloadsPath;
            }
            
            var filePath = Path.Combine(downloadsFolder, 
                $"PL_Statement_{ReportPeriodStartDate:yyyyMMdd}_to_{ReportPeriodEndDate:yyyyMMdd}.pdf");
            
            await File.WriteAllBytesAsync(filePath, pdfBytes);
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            SetError($"Failed to export P&L statement: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ExportBalanceSheetStatementAsync()
    {
        try
        {
            IsBusy = true;
            var pdfBytes = await _apiClient.GetBytesAsync(
                $"api/finance/reports/balance-sheet/export?asOfDate={BalanceSheetAsOfDate:yyyy-MM-dd}");
            
            var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var downloadsFolder = Path.Combine(downloadsPath, "Downloads");
            
            if (!Directory.Exists(downloadsFolder))
            {
                downloadsFolder = downloadsPath;
            }
            
            var filePath = Path.Combine(downloadsFolder, 
                $"Balance_Sheet_{BalanceSheetAsOfDate:yyyyMMdd}.pdf");
            
            await File.WriteAllBytesAsync(filePath, pdfBytes);
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            SetError($"Failed to export balance sheet: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task Calculate280EImpactAnalysisAsync()
    {
        try
        {
            IsBusy = true;
            var analysis = await _apiClient.GetAsync<dynamic>(
                $"api/finance/reports/280e-analysis?startDate={ReportPeriodStartDate:yyyy-MM-dd}&endDate={ReportPeriodEndDate:yyyy-MM-dd}");
            
            if (analysis != null)
            {
                Total280EDeductibleExpenses = analysis.deductible ?? 0m;
                Total280ENonDeductibleExpenses = analysis.nonDeductible ?? 0m;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to calculate 280E impact: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAllReportsAsync()
    {
        await GenerateProfitAndLossReportAsync();
        await GenerateBalanceSheetReportAsync();
    }

    partial void OnReportPeriodStartDateChanged(DateTime value)
    {
        if (ActiveReportView == "ProfitAndLoss")
        {
            _ = GenerateProfitAndLossReportAsync();
        }
    }

    partial void OnReportPeriodEndDateChanged(DateTime value)
    {
        if (ActiveReportView == "ProfitAndLoss")
        {
            _ = GenerateProfitAndLossReportAsync();
        }
    }

    partial void OnBalanceSheetAsOfDateChanged(DateTime value)
    {
        if (ActiveReportView == "BalanceSheet")
        {
            _ = GenerateBalanceSheetReportAsync();
        }
    }
}
