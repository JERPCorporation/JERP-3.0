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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JERP.Application.DTOs.Finance;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class VendorsViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<VendorDto> _supplierDirectory = new();

    [ObservableProperty]
    private VendorDto? _selectedSupplier;

    [ObservableProperty]
    private string _supplierSearchText = string.Empty;

    [ObservableProperty]
    private bool _showInactiveSuppliers = false;

    [ObservableProperty]
    private bool _cannabisVendorsOnlyFilter = false;

    [ObservableProperty]
    private int _totalSuppliersCount;

    [ObservableProperty]
    private int _activeSuppliersCount;

    [ObservableProperty]
    private int _cannabisLicensedSuppliersCount;

    [ObservableProperty]
    private int _suppliersAwaitingLicenseVerification;

    [ObservableProperty]
    private decimal _aggregateSupplierBalances;

    [ObservableProperty]
    private decimal _topSupplierExposure;

    [ObservableProperty]
    private string? _topSupplierName;

    [ObservableProperty]
    private int _currentPageIndex = 1;

    [ObservableProperty]
    private int _totalPageCount = 1;

    [ObservableProperty]
    private int _recordsPerPage = 25;

    public VendorsViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadVendorDirectoryAsync();
    }

    [RelayCommand]
    private async Task LoadVendorDirectoryAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/v1/vendors?page={CurrentPageIndex}&pageSize={RecordsPerPage}&includeInactive={ShowInactiveSuppliers}";
            
            if (!string.IsNullOrWhiteSpace(SupplierSearchText))
            {
                query += $"&search={Uri.EscapeDataString(SupplierSearchText)}";
            }

            if (CannabisVendorsOnlyFilter)
            {
                query += "&cannabisOnly=true";
            }

            var response = await _apiClient.GetAsync<dynamic>(query);
            
            if (response != null)
            {
                SupplierDirectory.Clear();
                TotalSuppliersCount = 0;
                ActiveSuppliersCount = 0;
                CannabisLicensedSuppliersCount = 0;
                SuppliersAwaitingLicenseVerification = 0;
                AggregateSupplierBalances = 0;
                TopSupplierExposure = 0;
                TopSupplierName = null;

                var vendors = response.items as IEnumerable<dynamic>;
                if (vendors != null)
                {
                    foreach (var vendor in vendors)
                    {
                        var vendorDto = new VendorDto
                        {
                            Id = vendor.id,
                            CompanyId = vendor.companyId,
                            VendorNumber = vendor.vendorNumber,
                            CompanyName = vendor.companyName,
                            ContactName = vendor.contactName,
                            Email = vendor.email,
                            Phone = vendor.phone,
                            Street = vendor.street,
                            City = vendor.city,
                            State = vendor.state,
                            PostalCode = vendor.postalCode,
                            Country = vendor.country,
                            TaxId = vendor.taxId,
                            PaymentTerms = vendor.paymentTerms ?? 30,
                            AccountsPayableAccountId = vendor.accountsPayableAccountId,
                            Balance = vendor.balance ?? 0m,
                            IsActive = vendor.isActive ?? true,
                            CannabisLicense = vendor.cannabisLicense,
                            IsCannabisVendor = vendor.isCannabisVendor ?? false,
                            Notes = vendor.notes,
                            CreatedAt = vendor.createdAt ?? DateTime.Now,
                            UpdatedAt = vendor.updatedAt ?? DateTime.Now
                        };

                        SupplierDirectory.Add(vendorDto);
                        TotalSuppliersCount++;

                        if (vendorDto.IsActive)
                        {
                            ActiveSuppliersCount++;
                        }

                        if (vendorDto.IsCannabisVendor)
                        {
                            CannabisLicensedSuppliersCount++;
                            
                            if (string.IsNullOrWhiteSpace(vendorDto.CannabisLicense))
                            {
                                SuppliersAwaitingLicenseVerification++;
                            }
                        }

                        AggregateSupplierBalances += vendorDto.Balance;

                        if (vendorDto.Balance > TopSupplierExposure)
                        {
                            TopSupplierExposure = vendorDto.Balance;
                            TopSupplierName = vendorDto.CompanyName;
                        }
                    }
                }

                TotalPageCount = response.totalPages ?? 1;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load vendor directory: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SearchSuppliersAsync()
    {
        CurrentPageIndex = 1;
        await LoadVendorDirectoryAsync();
    }

    [RelayCommand]
    private void RegisterNewSupplier()
    {
        // TODO: Open vendor registration dialog with cannabis license fields
    }

    [RelayCommand]
    private void EditSupplierProfile(VendorDto? vendor)
    {
        if (vendor == null) return;
        // TODO: Open vendor edit dialog
    }

    [RelayCommand]
    private async Task VerifyCannabisLicenseAsync(VendorDto? vendor)
    {
        if (vendor == null || !vendor.IsCannabisVendor) return;

        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>($"api/v1/vendors/{vendor.Id}/verify-license", new { });
            await LoadVendorDirectoryAsync();
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
    private async Task ToggleSupplierActiveStatusAsync(VendorDto? vendor)
    {
        if (vendor == null) return;

        try
        {
            IsBusy = true;
            await _apiClient.PutAsync<object>($"api/v1/vendors/{vendor.Id}/toggle-status", new { });
            await LoadVendorDirectoryAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to toggle supplier status: {ex.Message}");
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
            await LoadVendorDirectoryAsync();
        }
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (CurrentPageIndex > 1)
        {
            CurrentPageIndex--;
            await LoadVendorDirectoryAsync();
        }
    }

    partial void OnShowInactiveSuppliersChanged(bool value)
    {
        CurrentPageIndex = 1;
        _ = LoadVendorDirectoryAsync();
    }

    partial void OnCannabisVendorsOnlyFilterChanged(bool value)
    {
        CurrentPageIndex = 1;
        _ = LoadVendorDirectoryAsync();
    }
}
