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

namespace JERP.Desktop.ViewModels.Finance;

public partial class VendorsViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<VendorDto> _vendorList = new();

    [ObservableProperty]
    private VendorDto? _activeVendorSelection;

    [ObservableProperty]
    private string _filterQuery = string.Empty;

    [ObservableProperty]
    private bool _showOnlyActiveVendors = false;

    [ObservableProperty]
    private int _paginationIndex = 1;

    [ObservableProperty]
    private int _paginationTotal = 1;

    [ObservableProperty]
    private int _resultsPerPage = 25;

    public VendorsViewModel(IApiClient apiClient, IAuthenticationService authService)
    {
        _apiClient = apiClient;
        _authService = authService;
        _ = LoadVendorRecordsAsync();
    }

    [RelayCommand]
    private async Task LoadVendorRecordsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var businessId = _authService.CurrentUser?.CompanyId;
            if (businessId == null)
            {
                SetError("Business context is not set");
                return;
            }

            var apiPath = $"api/v1/vendors?companyId={businessId}&page={PaginationIndex}&pageSize={ResultsPerPage}";
            
            if (!string.IsNullOrWhiteSpace(FilterQuery))
            {
                apiPath += $"&search={Uri.EscapeDataString(FilterQuery)}";
            }

            if (ShowOnlyActiveVendors)
            {
                apiPath += "&activeOnly=true";
            }

            var apiResult = await _apiClient.GetAsync<dynamic>(apiPath);
            
            if (apiResult != null)
            {
                VendorList.Clear();
                
                var vendorItems = apiResult.items as IEnumerable<dynamic>;
                if (vendorItems != null)
                {
                    foreach (var vendorItem in vendorItems)
                    {
                        VendorList.Add(new VendorDto
                        {
                            Id = vendorItem.id,
                            CompanyId = vendorItem.companyId,
                            VendorNumber = vendorItem.vendorNumber,
                            CompanyName = vendorItem.companyName,
                            ContactName = vendorItem.contactName,
                            Email = vendorItem.email,
                            Phone = vendorItem.phone,
                            Street = vendorItem.street,
                            City = vendorItem.city,
                            State = vendorItem.state,
                            PostalCode = vendorItem.postalCode,
                            Country = vendorItem.country,
                            TaxId = vendorItem.taxId,
                            PaymentTerms = vendorItem.paymentTerms,
                            AccountsPayableAccountId = vendorItem.accountsPayableAccountId,
                            Balance = vendorItem.balance,
                            IsActive = vendorItem.isActive,
                            CannabisLicense = vendorItem.cannabisLicense,
                            IsCannabisVendor = vendorItem.isCannabisVendor,
                            Notes = vendorItem.notes,
                            CreatedAt = vendorItem.createdAt,
                            UpdatedAt = vendorItem.updatedAt
                        });
                    }
                }
                
                PaginationTotal = apiResult.totalPages ?? 1;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load vendor records: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ExecuteSearchAsync()
    {
        PaginationIndex = 1;
        await LoadVendorRecordsAsync();
    }

    [RelayCommand]
    private async Task ToggleActiveFilterAsync()
    {
        ShowOnlyActiveVendors = !ShowOnlyActiveVendors;
        PaginationIndex = 1;
        await LoadVendorRecordsAsync();
    }

    [RelayCommand]
    private void AddNewVendor()
    {
        // TODO: Launch vendor creation dialog
    }

    [RelayCommand]
    private void UpdateVendorInfo(VendorDto? vendorRecord)
    {
        if (vendorRecord == null) return;
        // TODO: Launch vendor editing dialog
    }

    [RelayCommand]
    private async Task RemoveVendorAsync(VendorDto? vendorRecord)
    {
        if (vendorRecord == null) return;

        try
        {
            var businessId = _authService.CurrentUser?.CompanyId;
            if (businessId == null)
            {
                SetError("Business context is not set");
                return;
            }

            await _apiClient.DeleteAsync($"api/v1/vendors/{vendorRecord.Id}");
            await LoadVendorRecordsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to remove vendor: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task MoveToNextPageAsync()
    {
        if (PaginationIndex < PaginationTotal)
        {
            PaginationIndex++;
            await LoadVendorRecordsAsync();
        }
    }

    [RelayCommand]
    private async Task MoveToPriorPageAsync()
    {
        if (PaginationIndex > 1)
        {
            PaginationIndex--;
            await LoadVendorRecordsAsync();
        }
    }
}
