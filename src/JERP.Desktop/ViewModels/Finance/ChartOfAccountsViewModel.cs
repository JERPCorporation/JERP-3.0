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

    [ObservableProperty]
    private bool _templateSelectorOpen;

    [ObservableProperty]
    private ObservableCollection<AccountTemplateSummaryDto> _availableTemplates = new();

    [ObservableProperty]
    private AccountTemplateSummaryDto? _selectedTemplate;

    [ObservableProperty]
    private bool _overwriteMode;

    [ObservableProperty]
    private string? _operationMessage;

    [ObservableProperty]
    private bool _processingImport;

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

            var requestPath = $"api/v1/finance/accounts?companyId={companyIdentifier}&page={PageIndex}&pageSize={RecordsPerPage}";
            
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

            await _apiClient.DeleteAsync($"api/v1/finance/accounts/{accountRecord.Id}");
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

    [RelayCommand]
    private async Task OpenTemplateSelectorAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var templateData = await _apiClient.GetAsync<List<AccountTemplateSummaryDto>>("api/v1/finance/account-templates");
            
            if (templateData != null)
            {
                AvailableTemplates.Clear();
                foreach (var item in templateData)
                {
                    AvailableTemplates.Add(item);
                }
                TemplateSelectorOpen = true;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed loading templates: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void CloseTemplateSelector()
    {
        TemplateSelectorOpen = false;
        SelectedTemplate = null;
        OverwriteMode = false;
        OperationMessage = null;
    }

    [RelayCommand]
    private void ChooseTemplate(AccountTemplateSummaryDto? template)
    {
        SelectedTemplate = template;
    }

    [RelayCommand]
    private async Task PerformTemplateImportAsync()
    {
        if (SelectedTemplate == null || ProcessingImport) return;

        ProcessingImport = true;
        ErrorMessage = null;
        OperationMessage = null;

        try
        {
            var organizationId = _authService.CurrentUser?.CompanyId;
            if (organizationId == null)
            {
                SetError("Organization ID not available");
                return;
            }

            var requestData = new
            {
                CompanyId = organizationId,
                OverwriteExisting = OverwriteMode
            };

            var operationResult = await _apiClient.PostAsync<TemplateLoadResultDto>(
                $"api/v1/finance/account-templates/{SelectedTemplate.Code}/load", 
                requestData);

            if (operationResult != null && operationResult.Success)
            {
                OperationMessage = $"Import successful: {operationResult.AccountsCreated} accounts created";
                await Task.Delay(1500);
                TemplateSelectorOpen = false;
                await FetchAccountsDataAsync();
            }
            else if (operationResult != null)
            {
                SetError($"Import errors: {string.Join(", ", operationResult.Errors)}");
            }
        }
        catch (Exception ex)
        {
            SetError($"Import operation failed: {ex.Message}");
        }
        finally
        {
            ProcessingImport = false;
        }
    }
}
