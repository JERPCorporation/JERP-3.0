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

namespace JERP.Desktop.ViewModels;

public partial class CustomersViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<CustomerDto> _clientPortfolio = new();

    [ObservableProperty]
    private CustomerDto? _selectedClient;

    [ObservableProperty]
    private string _clientSearchText = string.Empty;

    [ObservableProperty]
    private bool _showInactiveClients = false;

    [ObservableProperty]
    private bool _cannabisCustomersOnlyFilter = false;

    [ObservableProperty]
    private bool _creditLimitExceededFilter = false;

    [ObservableProperty]
    private int _totalClientsCount;

    [ObservableProperty]
    private int _activeClientsCount;

    [ObservableProperty]
    private int _cannabisLicensedClientsCount;

    [ObservableProperty]
    private int _clientsExceedingCreditLimit;

    [ObservableProperty]
    private int _clientsNearingCreditLimit;

    [ObservableProperty]
    private decimal _aggregateClientBalances;

    [ObservableProperty]
    private decimal _totalCreditLimitExtended;

    [ObservableProperty]
    private decimal _availableCreditCapacity;

    [ObservableProperty]
    private decimal _largestClientExposure;

    [ObservableProperty]
    private string? _largestClientName;

    [ObservableProperty]
    private int _currentPageIndex = 1;

    [ObservableProperty]
    private int _totalPageCount = 1;

    [ObservableProperty]
    private int _recordsPerPage = 25;

    public CustomersViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadCustomerPortfolioAsync();
    }

    [RelayCommand]
    private async Task LoadCustomerPortfolioAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/finance/customers?page={CurrentPageIndex}&pageSize={RecordsPerPage}&includeInactive={ShowInactiveClients}";
            
            if (!string.IsNullOrWhiteSpace(ClientSearchText))
            {
                query += $"&search={Uri.EscapeDataString(ClientSearchText)}";
            }

            if (CannabisCustomersOnlyFilter)
            {
                query += "&cannabisOnly=true";
            }

            if (CreditLimitExceededFilter)
            {
                query += "&creditExceeded=true";
            }

            var response = await _apiClient.GetAsync<dynamic>(query);
            
            if (response != null)
            {
                ClientPortfolio.Clear();
                TotalClientsCount = 0;
                ActiveClientsCount = 0;
                CannabisLicensedClientsCount = 0;
                ClientsExceedingCreditLimit = 0;
                ClientsNearingCreditLimit = 0;
                AggregateClientBalances = 0;
                TotalCreditLimitExtended = 0;
                AvailableCreditCapacity = 0;
                LargestClientExposure = 0;
                LargestClientName = null;

                var customers = response.items as IEnumerable<dynamic>;
                if (customers != null)
                {
                    foreach (var customer in customers)
                    {
                        var customerDto = new CustomerDto
                        {
                            Id = customer.id,
                            CompanyId = customer.companyId,
                            CustomerNumber = customer.customerNumber,
                            CompanyName = customer.companyName,
                            ContactName = customer.contactName,
                            Email = customer.email,
                            Phone = customer.phone,
                            Street = customer.street,
                            City = customer.city,
                            State = customer.state,
                            PostalCode = customer.postalCode,
                            Country = customer.country,
                            TaxId = customer.taxId,
                            PaymentTerms = customer.paymentTerms ?? 30,
                            AccountsReceivableAccountId = customer.accountsReceivableAccountId,
                            Balance = customer.balance ?? 0m,
                            CreditLimit = customer.creditLimit ?? 0m,
                            IsActive = customer.isActive ?? true,
                            CannabisLicense = customer.cannabisLicense,
                            IsCannabisCustomer = customer.isCannabisCustomer ?? false,
                            Notes = customer.notes,
                            CreatedAt = customer.createdAt ?? DateTime.Now,
                            UpdatedAt = customer.updatedAt ?? DateTime.Now
                        };

                        ClientPortfolio.Add(customerDto);
                        TotalClientsCount++;

                        if (customerDto.IsActive)
                        {
                            ActiveClientsCount++;
                        }

                        if (customerDto.IsCannabisCustomer)
                        {
                            CannabisLicensedClientsCount++;
                        }

                        AggregateClientBalances += customerDto.Balance;
                        TotalCreditLimitExtended += customerDto.CreditLimit;
                        AvailableCreditCapacity += customerDto.AvailableCredit;

                        if (customerDto.Balance > customerDto.CreditLimit && customerDto.CreditLimit > 0)
                        {
                            ClientsExceedingCreditLimit++;
                        }
                        else if (customerDto.CreditLimit > 0 && customerDto.AvailableCredit < (customerDto.CreditLimit * 0.1m))
                        {
                            ClientsNearingCreditLimit++;
                        }

                        if (customerDto.Balance > LargestClientExposure)
                        {
                            LargestClientExposure = customerDto.Balance;
                            LargestClientName = customerDto.CompanyName;
                        }
                    }
                }

                TotalPageCount = response.totalPages ?? 1;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load customer portfolio: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SearchClientsAsync()
    {
        CurrentPageIndex = 1;
        await LoadCustomerPortfolioAsync();
    }

    [RelayCommand]
    private void RegisterNewClient()
    {
        // TODO: Open customer registration dialog with credit limit
    }

    [RelayCommand]
    private void EditClientProfile(CustomerDto? customer)
    {
        if (customer == null) return;
        // TODO: Open customer edit dialog
    }

    [RelayCommand]
    private void AdjustCreditLimitAsync(CustomerDto? customer)
    {
        if (customer == null) return;
        // TODO: Open credit limit adjustment dialog
    }

    [RelayCommand]
    private async Task MonitorCreditLimitExposureAsync()
    {
        try
        {
            IsBusy = true;
            var exposure = await _apiClient.GetAsync<dynamic>("api/finance/customers/credit-exposure");
            
            if (exposure != null)
            {
                ClientsExceedingCreditLimit = exposure.exceededCount ?? 0;
                ClientsNearingCreditLimit = exposure.nearingCount ?? 0;
                TotalCreditLimitExtended = exposure.totalExtended ?? 0m;
                AvailableCreditCapacity = exposure.available ?? 0m;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to monitor credit exposure: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task VerifyCannabisLicenseAsync(CustomerDto? customer)
    {
        if (customer == null || !customer.IsCannabisCustomer) return;

        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>($"api/finance/customers/{customer.Id}/verify-license", new { });
            await LoadCustomerPortfolioAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to verify cannabis license: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ToggleClientActiveStatusAsync(CustomerDto? customer)
    {
        if (customer == null) return;

        try
        {
            IsBusy = true;
            await _apiClient.PutAsync($"api/finance/customers/{customer.Id}/toggle-status", new { });
            await LoadCustomerPortfolioAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to toggle client status: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (CurrentPageIndex < TotalPageCount)
        {
            CurrentPageIndex++;
            await LoadCustomerPortfolioAsync();
        }
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (CurrentPageIndex > 1)
        {
            CurrentPageIndex--;
            await LoadCustomerPortfolioAsync();
        }
    }

    partial void OnShowInactiveClientsChanged(bool value)
    {
        CurrentPageIndex = 1;
        _ = LoadCustomerPortfolioAsync();
    }

    partial void OnCannabisCustomersOnlyFilterChanged(bool value)
    {
        CurrentPageIndex = 1;
        _ = LoadCustomerPortfolioAsync();
    }

    partial void OnCreditLimitExceededFilterChanged(bool value)
    {
        CurrentPageIndex = 1;
        _ = LoadCustomerPortfolioAsync();
    }
}
