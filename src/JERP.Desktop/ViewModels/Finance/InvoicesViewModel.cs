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
using JERP.Core.Enums;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels.Finance;

public partial class InvoicesViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<InvoiceDto> _invoiceRepository = new();

    [ObservableProperty]
    private InvoiceDto? _focusedInvoice;

    [ObservableProperty]
    private string _textFilter = string.Empty;

    [ObservableProperty]
    private InvoiceStatus? _invoiceStatusSelector;

    [ObservableProperty]
    private int _currentPageIndex = 1;

    [ObservableProperty]
    private int _maximumPageIndex = 1;

    [ObservableProperty]
    private int _recordsPerView = 25;

    public InvoicesViewModel(IApiClient apiClient, IAuthenticationService authService)
    {
        _apiClient = apiClient;
        _authService = authService;
        _ = AcquireInvoiceDataAsync();
    }

    [RelayCommand]
    private async Task AcquireInvoiceDataAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var entityId = _authService.CurrentUser?.CompanyId;
            if (entityId == null)
            {
                SetError("Entity identifier missing");
                return;
            }

            var urlPath = $"api/v1/finance/invoices?companyId={entityId}&page={CurrentPageIndex}&pageSize={RecordsPerView}";
            
            if (!string.IsNullOrWhiteSpace(TextFilter))
            {
                urlPath += $"&search={Uri.EscapeDataString(TextFilter)}";
            }

            if (InvoiceStatusSelector.HasValue)
            {
                urlPath += $"&status={InvoiceStatusSelector.Value}";
            }

            var httpResponse = await _apiClient.GetAsync<dynamic>(urlPath);
            
            if (httpResponse != null)
            {
                InvoiceRepository.Clear();
                
                var invoiceSet = httpResponse.items as IEnumerable<dynamic>;
                if (invoiceSet != null)
                {
                    foreach (var invoiceData in invoiceSet)
                    {
                        var invoice = new InvoiceDto
                        {
                            Id = invoiceData.id,
                            CompanyId = invoiceData.companyId,
                            CustomerId = invoiceData.customerId,
                            CustomerName = invoiceData.customerName,
                            CustomerNumber = invoiceData.customerNumber,
                            InvoiceNumber = invoiceData.invoiceNumber,
                            InvoiceDate = invoiceData.invoiceDate,
                            DueDate = invoiceData.dueDate,
                            Subtotal = invoiceData.subtotal,
                            TaxAmount = invoiceData.taxAmount,
                            TotalAmount = invoiceData.totalAmount,
                            AmountPaid = invoiceData.amountPaid,
                            AmountDue = invoiceData.amountDue,
                            StatusEnum = (InvoiceStatus)invoiceData.status,
                            Status = invoiceData.status?.ToString() ?? string.Empty,
                            IsPaid = invoiceData.isPaid,
                            PaymentDate = invoiceData.paymentDate,
                            JournalEntryId = invoiceData.journalEntryId,
                            Notes = invoiceData.notes,
                            CreatedAt = invoiceData.createdAt,
                            UpdatedAt = invoiceData.updatedAt
                        };

                        if (invoiceData.lineItems != null)
                        {
                            foreach (var itemLine in invoiceData.lineItems)
                            {
                                invoice.LineItems.Add(new InvoiceLineItemDto
                                {
                                    Id = itemLine.id,
                                    InvoiceId = itemLine.invoiceId,
                                    AccountId = itemLine.accountId,
                                    AccountNumber = itemLine.accountNumber,
                                    AccountName = itemLine.accountName,
                                    Description = itemLine.description,
                                    Quantity = itemLine.quantity,
                                    UnitPrice = itemLine.unitPrice,
                                    Amount = itemLine.amount
                                });
                            }
                        }

                        if (invoiceData.payments != null)
                        {
                            foreach (var paymentInfo in invoiceData.payments)
                            {
                                invoice.Payments.Add(new InvoicePaymentDto
                                {
                                    Id = paymentInfo.id,
                                    CompanyId = paymentInfo.companyId,
                                    InvoiceId = paymentInfo.invoiceId,
                                    ReceiptNumber = paymentInfo.receiptNumber,
                                    PaymentDate = paymentInfo.paymentDate,
                                    Amount = paymentInfo.amount,
                                    PaymentMethod = paymentInfo.paymentMethod,
                                    ReferenceNumber = paymentInfo.referenceNumber,
                                    JournalEntryId = paymentInfo.journalEntryId,
                                    Notes = paymentInfo.notes
                                });
                            }
                        }

                        InvoiceRepository.Add(invoice);
                    }
                }
                
                MaximumPageIndex = httpResponse.totalPages ?? 1;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to acquire invoice data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ExecuteFilterAsync()
    {
        CurrentPageIndex = 1;
        await AcquireInvoiceDataAsync();
    }

    [RelayCommand]
    private async Task SelectInvoiceStatusAsync(InvoiceStatus? statusOption)
    {
        InvoiceStatusSelector = statusOption;
        CurrentPageIndex = 1;
        await AcquireInvoiceDataAsync();
    }

    [RelayCommand]
    private void GenerateNewInvoice()
    {
        // TODO: Launch invoice creation dialog
    }

    [RelayCommand]
    private void ReviseInvoiceData(InvoiceDto? invoiceRecord)
    {
        if (invoiceRecord == null) return;
        // TODO: Launch invoice editing dialog
    }

    [RelayCommand]
    private async Task EradicateInvoiceAsync(InvoiceDto? invoiceRecord)
    {
        if (invoiceRecord == null) return;

        try
        {
            var entityId = _authService.CurrentUser?.CompanyId;
            if (entityId == null)
            {
                SetError("Entity identifier missing");
                return;
            }

            await _apiClient.DeleteAsync($"api/v1/finance/invoices/{invoiceRecord.Id}");
            await AcquireInvoiceDataAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to eradicate invoice: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task AdvancePageIndexAsync()
    {
        if (CurrentPageIndex < MaximumPageIndex)
        {
            CurrentPageIndex++;
            await AcquireInvoiceDataAsync();
        }
    }

    [RelayCommand]
    private async Task RetreatPageIndexAsync()
    {
        if (CurrentPageIndex > 1)
        {
            CurrentPageIndex--;
            await AcquireInvoiceDataAsync();
        }
    }
}
