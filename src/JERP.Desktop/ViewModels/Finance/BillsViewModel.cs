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

public partial class BillsViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<BillDto> _billRecords = new();

    [ObservableProperty]
    private BillDto? _selectedBillItem;

    [ObservableProperty]
    private string _searchInput = string.Empty;

    [ObservableProperty]
    private BillStatus? _billStatusFilter;

    [ObservableProperty]
    private int _activePage = 1;

    [ObservableProperty]
    private int _lastPage = 1;

    [ObservableProperty]
    private int _entriesPerPage = 25;

    public BillsViewModel(IApiClient apiClient, IAuthenticationService authService)
    {
        _apiClient = apiClient;
        _authService = authService;
        _ = FetchBillDataAsync();
    }

    [RelayCommand]
    private async Task FetchBillDataAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var companyRef = _authService.CurrentUser?.CompanyId;
            if (companyRef == null)
            {
                SetError("Company reference not found");
                return;
            }

            var queryString = $"api/v1/vendors/bills?companyId={companyRef}&page={ActivePage}&pageSize={EntriesPerPage}";
            
            if (!string.IsNullOrWhiteSpace(SearchInput))
            {
                queryString += $"&search={Uri.EscapeDataString(SearchInput)}";
            }

            if (BillStatusFilter.HasValue)
            {
                queryString += $"&status={BillStatusFilter.Value}";
            }

            var serviceResponse = await _apiClient.GetAsync<dynamic>(queryString);
            
            if (serviceResponse != null)
            {
                BillRecords.Clear();
                
                var billData = serviceResponse.items as IEnumerable<dynamic>;
                if (billData != null)
                {
                    foreach (var billEntry in billData)
                    {
                        var bill = new BillDto
                        {
                            Id = billEntry.id,
                            CompanyId = billEntry.companyId,
                            VendorId = billEntry.vendorId,
                            VendorName = billEntry.vendorName,
                            VendorNumber = billEntry.vendorNumber,
                            BillNumber = billEntry.billNumber,
                            VendorInvoiceNumber = billEntry.vendorInvoiceNumber,
                            BillDate = billEntry.billDate,
                            DueDate = billEntry.dueDate,
                            Subtotal = billEntry.subtotal,
                            SubTotal = billEntry.subtotal,
                            TaxAmount = billEntry.taxAmount,
                            TotalAmount = billEntry.totalAmount,
                            AmountPaid = billEntry.amountPaid,
                            AmountDue = billEntry.amountDue,
                            StatusEnum = (BillStatus)billEntry.status,
                            Status = billEntry.status?.ToString() ?? string.Empty,
                            IsPaid = billEntry.isPaid,
                            PaymentDate = billEntry.paymentDate,
                            JournalEntryId = billEntry.journalEntryId,
                            Notes = billEntry.notes,
                            CreatedAt = billEntry.createdAt,
                            UpdatedAt = billEntry.updatedAt
                        };

                        if (billEntry.lineItems != null)
                        {
                            foreach (var lineItem in billEntry.lineItems)
                            {
                                bill.LineItems.Add(new BillLineItemDto
                                {
                                    Id = lineItem.id,
                                    BillId = lineItem.billId,
                                    AccountId = lineItem.accountId,
                                    AccountNumber = lineItem.accountNumber,
                                    AccountName = lineItem.accountName,
                                    Description = lineItem.description,
                                    Quantity = lineItem.quantity,
                                    UnitPrice = lineItem.unitPrice,
                                    Amount = lineItem.amount,
                                    IsCOGS = lineItem.isCOGS
                                });
                            }
                        }

                        if (billEntry.payments != null)
                        {
                            foreach (var payment in billEntry.payments)
                            {
                                bill.Payments.Add(new BillPaymentDto
                                {
                                    Id = payment.id,
                                    CompanyId = payment.companyId,
                                    BillId = payment.billId,
                                    PaymentNumber = payment.paymentNumber,
                                    PaymentDate = payment.paymentDate,
                                    Amount = payment.amount,
                                    PaymentMethod = payment.paymentMethod,
                                    ReferenceNumber = payment.referenceNumber,
                                    JournalEntryId = payment.journalEntryId,
                                    Notes = payment.notes
                                });
                            }
                        }

                        BillRecords.Add(bill);
                    }
                }
                
                LastPage = serviceResponse.totalPages ?? 1;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to fetch bill data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RunSearchQueryAsync()
    {
        ActivePage = 1;
        await FetchBillDataAsync();
    }

    [RelayCommand]
    private async Task ApplyStatusFilterAsync(BillStatus? statusValue)
    {
        BillStatusFilter = statusValue;
        ActivePage = 1;
        await FetchBillDataAsync();
    }

    [RelayCommand]
    private void CreateBillEntry()
    {
        // TODO: Launch bill creation dialog
    }

    [RelayCommand]
    private void ModifyBillEntry(BillDto? billItem)
    {
        if (billItem == null) return;
        // TODO: Launch bill editing dialog
    }

    [RelayCommand]
    private async Task DeleteBillEntryAsync(BillDto? billItem)
    {
        if (billItem == null) return;

        try
        {
            var companyRef = _authService.CurrentUser?.CompanyId;
            if (companyRef == null)
            {
                SetError("Company reference not found");
                return;
            }

            await _apiClient.DeleteAsync($"api/v1/vendors/bills/{billItem.Id}");
            await FetchBillDataAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to delete bill: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task GoToNextPageAsync()
    {
        if (ActivePage < LastPage)
        {
            ActivePage++;
            await FetchBillDataAsync();
        }
    }

    [RelayCommand]
    private async Task GoToPreviousPageAsync()
    {
        if (ActivePage > 1)
        {
            ActivePage--;
            await FetchBillDataAsync();
        }
    }
}
