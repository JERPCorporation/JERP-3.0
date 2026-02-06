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
using JERP.Application.DTOs.Finance;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class ChartOfAccountsViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<AccountDto> _generalLedgerAccounts = new();

    [ObservableProperty]
    private AccountDto? _selectedLedgerAccount;

    [ObservableProperty]
    private string _accountSearchFilter = string.Empty;

    [ObservableProperty]
    private string _accountTypeFilter = "All";

    [ObservableProperty]
    private bool _showInactiveAccounts = false;

    [ObservableProperty]
    private int _assetAccountsCount;

    [ObservableProperty]
    private int _liabilityAccountsCount;

    [ObservableProperty]
    private int _equityAccountsCount;

    [ObservableProperty]
    private int _revenueAccountsCount;

    [ObservableProperty]
    private int _expenseAccountsCount;

    [ObservableProperty]
    private int _fasbCompliantAccountsCount;

    [ObservableProperty]
    private int _accountsMissingFasbMapping;

    [ObservableProperty]
    private decimal _totalAssetBalance;

    [ObservableProperty]
    private decimal _totalLiabilityBalance;

    [ObservableProperty]
    private decimal _totalEquityBalance;

    public ChartOfAccountsViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadChartOfAccountsAsync();
    }

    [RelayCommand]
    private async Task LoadChartOfAccountsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/v1/finance/accounts?includeInactive={ShowInactiveAccounts}";
            
            if (!string.IsNullOrWhiteSpace(AccountSearchFilter))
            {
                query += $"&search={Uri.EscapeDataString(AccountSearchFilter)}";
            }

            if (AccountTypeFilter != "All")
            {
                query += $"&type={AccountTypeFilter}";
            }

            var accounts = await _apiClient.GetAsync<List<AccountDto>>(query);
            
            if (accounts != null)
            {
                GeneralLedgerAccounts.Clear();
                AssetAccountsCount = 0;
                LiabilityAccountsCount = 0;
                EquityAccountsCount = 0;
                RevenueAccountsCount = 0;
                ExpenseAccountsCount = 0;
                FasbCompliantAccountsCount = 0;
                AccountsMissingFasbMapping = 0;
                TotalAssetBalance = 0;
                TotalLiabilityBalance = 0;
                TotalEquityBalance = 0;

                foreach (var account in accounts)
                {
                    GeneralLedgerAccounts.Add(account);
                    
                    switch (account.Type.ToString())
                    {
                        case "Asset":
                            AssetAccountsCount++;
                            TotalAssetBalance += account.Balance;
                            break;
                        case "Liability":
                            LiabilityAccountsCount++;
                            TotalLiabilityBalance += account.Balance;
                            break;
                        case "Equity":
                            EquityAccountsCount++;
                            TotalEquityBalance += account.Balance;
                            break;
                        case "Revenue":
                            RevenueAccountsCount++;
                            break;
                        case "Expense":
                            ExpenseAccountsCount++;
                            break;
                    }

                    if (account.FASBTopicId != Guid.Empty && account.FASBTopicId != default(Guid))
                    {
                        FasbCompliantAccountsCount++;
                    }
                    else
                    {
                        AccountsMissingFasbMapping++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load chart of accounts: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task FilterAccountsByTypeAsync(string accountType)
    {
        AccountTypeFilter = accountType;
        await LoadChartOfAccountsAsync();
    }

    [RelayCommand]
    private void CreateNewGLAccount()
    {
        // TODO: Open account creation dialog with FASB topic selector
    }

    [RelayCommand]
    private void EditGLAccount(AccountDto? account)
    {
        if (account == null) return;
        // TODO: Open account edit dialog
    }

    [RelayCommand]
    private async Task ToggleAccountActiveStatusAsync(AccountDto? account)
    {
        if (account == null) return;

        try
        {
            IsBusy = true;
            await _apiClient.PutAsync($"api/v1/finance/accounts/{account.Id}/toggle-status", new { });
            await LoadChartOfAccountsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to toggle account status: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ReconcileAccountBalancesAsync()
    {
        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>("api/v1/finance/accounts/reconcile", new { });
            await LoadChartOfAccountsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to reconcile account balances: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ExportChartOfAccountsAsync()
    {
        try
        {
            IsBusy = true;
            var csvBytes = await _apiClient.GetBytesAsync("api/v1/finance/accounts/export");
            
            var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var downloadsFolder = Path.Combine(downloadsPath, "Downloads");
            
            if (!Directory.Exists(downloadsFolder))
            {
                downloadsFolder = downloadsPath;
            }
            
            var filePath = System.IO.Path.Combine(downloadsFolder, $"ChartOfAccounts_{DateTime.Now:yyyyMMdd}.csv");
            
            await System.IO.File.WriteAllBytesAsync(filePath, csvBytes);
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = System.IO.Path.GetDirectoryName(filePath)!,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            SetError($"Failed to export chart of accounts: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnAccountSearchFilterChanged(string value)
    {
        _ = LoadChartOfAccountsAsync();
    }

    partial void OnShowInactiveAccountsChanged(bool value)
    {
        _ = LoadChartOfAccountsAsync();
    }
}
