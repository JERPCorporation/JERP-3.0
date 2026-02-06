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
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels.Finance;

public partial class CustomersViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<CustomerDto> _customerDataset = new();

    [ObservableProperty]
    private CustomerDto? _highlightedCustomer;

    [ObservableProperty]
    private string _keywordSearch = string.Empty;

    [ObservableProperty]
    private int _pageNumber = 1;

    [ObservableProperty]
    private int _totalPages = 1;

    [ObservableProperty]
    private int _itemsPerPage = 25;

    public CustomersViewModel(IApiClient apiClient, IAuthenticationService authService)
    {
        _apiClient = apiClient;
        _authService = authService;
        _ = RetrieveCustomerDataAsync();
    }

    [RelayCommand]
    private async Task RetrieveCustomerDataAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var firmIdentifier = _authService.CurrentUser?.CompanyId;
            if (firmIdentifier == null)
            {
                SetError("Firm identifier not available");
                return;
            }

            var requestUri = $"api/v1/finance/customers?companyId={firmIdentifier}&page={PageNumber}&pageSize={ItemsPerPage}";
            
            if (!string.IsNullOrWhiteSpace(KeywordSearch))
            {
                requestUri += $"&search={Uri.EscapeDataString(KeywordSearch)}";
            }

            var dataResponse = await _apiClient.GetAsync<dynamic>(requestUri);
            
            if (dataResponse != null)
            {
                CustomerDataset.Clear();
                
                var customerRecords = dataResponse.items as IEnumerable<dynamic>;
                if (customerRecords != null)
                {
                    foreach (var customerRecord in customerRecords)
                    {
                        CustomerDataset.Add(new CustomerDto
                        {
                            Id = customerRecord.id,
                            CompanyId = customerRecord.companyId,
                            CustomerNumber = customerRecord.customerNumber,
                            CompanyName = customerRecord.companyName,
                            ContactName = customerRecord.contactName,
                            Email = customerRecord.email,
                            Phone = customerRecord.phone,
                            Street = customerRecord.street,
                            City = customerRecord.city,
                            State = customerRecord.state,
                            PostalCode = customerRecord.postalCode,
                            Country = customerRecord.country,
                            TaxId = customerRecord.taxId,
                            PaymentTerms = customerRecord.paymentTerms,
                            AccountsReceivableAccountId = customerRecord.accountsReceivableAccountId,
                            Balance = customerRecord.balance,
                            CreditLimit = customerRecord.creditLimit,
                            IsActive = customerRecord.isActive,
                            CannabisLicense = customerRecord.cannabisLicense,
                            IsCannabisCustomer = customerRecord.isCannabisCustomer,
                            Notes = customerRecord.notes,
                            CreatedAt = customerRecord.createdAt,
                            UpdatedAt = customerRecord.updatedAt
                        });
                    }
                }
                
                TotalPages = dataResponse.totalPages ?? 1;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to retrieve customer data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task InitiateSearchAsync()
    {
        PageNumber = 1;
        await RetrieveCustomerDataAsync();
    }

    [RelayCommand]
    private void RegisterNewCustomer()
    {
        // TODO: Launch customer creation dialog
    }

    [RelayCommand]
    private void AlterCustomerDetails(CustomerDto? customerRecord)
    {
        if (customerRecord == null) return;
        // TODO: Launch customer editing dialog
    }

    [RelayCommand]
    private async Task PurgeCustomerAsync(CustomerDto? customerRecord)
    {
        if (customerRecord == null) return;

        try
        {
            var firmIdentifier = _authService.CurrentUser?.CompanyId;
            if (firmIdentifier == null)
            {
                SetError("Firm identifier not available");
                return;
            }

            await _apiClient.DeleteAsync($"api/v1/finance/customers/{customerRecord.Id}");
            await RetrieveCustomerDataAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to purge customer: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task ProgressToNextPageAsync()
    {
        if (PageNumber < TotalPages)
        {
            PageNumber++;
            await RetrieveCustomerDataAsync();
        }
    }

    [RelayCommand]
    private async Task RegressToPreviousPageAsync()
    {
        if (PageNumber > 1)
        {
            PageNumber--;
            await RetrieveCustomerDataAsync();
        }
    }
}
