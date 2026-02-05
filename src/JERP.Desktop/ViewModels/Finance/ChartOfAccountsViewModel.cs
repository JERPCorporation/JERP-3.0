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
using JERP.Application.DTOs.Finance;
using JERP.Core.Enums;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels.Finance;

public partial class ChartOfAccountsViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<AccountDto> _accountRegistry = new();

    [ObservableProperty]
    private AccountDto? _selectedAccountRecord;

    [ObservableProperty]
    private string _queryText = string.Empty;

    [ObservableProperty]
    private AccountType? _accountCategoryFilter;

    [ObservableProperty]
    private int _pageIndex = 1;

    [ObservableProperty]
    private int _totalPageCount = 1;

    [ObservableProperty]
    private int _recordsPerPage = 25;

    public ChartOfAccountsViewModel(IApiClient apiClient, IAuthenticationService authService)
    {
        _apiClient = apiClient;
        _authService = authService;
        _ = FetchAccountsDataAsync();
    }

    [RelayCommand]
    private async Task FetchAccountsDataAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var companyIdentifier = _authService.CurrentUser?.CompanyId;
            if (companyIdentifier == null)
            {
                SetError("Company context not available");
                return;
            }

            var requestPath = $"api/finance/accounts?companyId={companyIdentifier}&page={PageIndex}&pageSize={RecordsPerPage}";
            
            if (!string.IsNullOrWhiteSpace(QueryText))
            {
                requestPath += $"&search={Uri.EscapeDataString(QueryText)}";
            }

            if (AccountCategoryFilter.HasValue)
            {
                requestPath += $"&type={AccountCategoryFilter.Value}";
            }

            var apiResponse = await _apiClient.GetAsync<dynamic>(requestPath);
            
            if (apiResponse != null)
            {
                AccountRegistry.Clear();
                
                var dataItems = apiResponse.items as IEnumerable<dynamic>;
                if (dataItems != null)
                {
                    foreach (var dataItem in dataItems)
                    {
                        AccountRegistry.Add(new AccountDto
                        {
                            Id = dataItem.id,
                            CompanyId = dataItem.companyId,
                            AccountNumber = dataItem.accountNumber,
                            AccountName = dataItem.accountName,
                            Type = (AccountType)dataItem.type,
                            Balance = dataItem.balance,
                            IsActive = dataItem.isActive,
                            IsSystemAccount = dataItem.isSystemAccount,
                            IsCOGS = dataItem.isCOGS,
                            IsNonDeductible = dataItem.isNonDeductible,
                            TaxCategory = dataItem.taxCategory,
                            FASBTopicId = dataItem.fasbTopicId,
                            FASBSubtopicId = dataItem.fasbSubtopicId,
                            FASBReference = dataItem.fasbReference,
                            FASBTopicName = dataItem.fasbTopicName,
                            FASBSubtopicName = dataItem.fasbSubtopicName,
                            FASBCategory = dataItem.fasbCategory,
                            CreatedAt = dataItem.createdAt,
                            UpdatedAt = dataItem.updatedAt
                        });
                    }
                }
                
                TotalPageCount = apiResponse.totalPages ?? 1;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to retrieve account data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ExecuteQueryAsync()
    {
        PageIndex = 1;
        await FetchAccountsDataAsync();
    }

    [RelayCommand]
    private async Task ApplyAccountTypeFilterAsync(AccountType? accountType)
    {
        AccountCategoryFilter = accountType;
        PageIndex = 1;
        await FetchAccountsDataAsync();
    }

    [RelayCommand]
    private void InitiateAccountCreation()
    {
        // TODO: Launch account creation dialog
    }

    [RelayCommand]
    private void ModifyAccountDetails(AccountDto? accountRecord)
    {
        if (accountRecord == null) return;
        // TODO: Launch account editing dialog
    }

    [RelayCommand]
    private async Task RemoveAccountAsync(AccountDto? accountRecord)
    {
        if (accountRecord == null) return;

        try
        {
            var companyIdentifier = _authService.CurrentUser?.CompanyId;
            if (companyIdentifier == null)
            {
                SetError("Company context not available");
                return;
            }

            await _apiClient.DeleteAsync($"api/finance/accounts/{accountRecord.Id}");
            await FetchAccountsDataAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to remove account: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task AdvanceToNextPageAsync()
    {
        if (PageIndex < TotalPageCount)
        {
            PageIndex++;
            await FetchAccountsDataAsync();
        }
    }

    [RelayCommand]
    private async Task ReturnToPreviousPageAsync()
    {
        if (PageIndex > 1)
        {
            PageIndex--;
            await FetchAccountsDataAsync();
        }
    }
}
