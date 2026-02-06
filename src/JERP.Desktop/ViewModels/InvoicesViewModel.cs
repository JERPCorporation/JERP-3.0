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

public partial class InvoicesViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<InvoiceListDto> _receivableDocuments = new();

    [ObservableProperty]
    private InvoiceDto? _selectedReceivableDocument;

    [ObservableProperty]
    private DateTime _invoiceDateFilterFrom = DateTime.Now.AddMonths(-3);

    [ObservableProperty]
    private DateTime _invoiceDateFilterTo = DateTime.Now.AddMonths(1);

    [ObservableProperty]
    private string _invoiceStatusFilter = "All";

    [ObservableProperty]
    private int _totalInvoicesCount;

    [ObservableProperty]
    private int _draftInvoicesCount;

    [ObservableProperty]
    private int _sentInvoicesCount;

    [ObservableProperty]
    private int _paidInvoicesCount;

    [ObservableProperty]
    private int _overdueInvoicesCount;

    [ObservableProperty]
    private decimal _totalReceivablesOutstanding;

    [ObservableProperty]
    private decimal _amountDueInAgingBucket0to30;

    [ObservableProperty]
    private decimal _amountDueInAgingBucket31to60;

    [ObservableProperty]
    private decimal _amountDueInAgingBucket61to90;

    [ObservableProperty]
    private decimal _amountDueInAgingBucketOver90;

    [ObservableProperty]
    private decimal _collectionEfficiencyRatio;

    [ObservableProperty]
    private decimal _totalCollectionsThisPeriod;

    [ObservableProperty]
    private int _invoicesCollectedThisPeriodCount;

    public InvoicesViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadInvoicesReceivableAsync();
    }

    [RelayCommand]
    private async Task LoadInvoicesReceivableAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/v1/finance/invoices?dateFrom={InvoiceDateFilterFrom:yyyy-MM-dd}&dateTo={InvoiceDateFilterTo:yyyy-MM-dd}";
            
            if (InvoiceStatusFilter != "All")
            {
                query += $"&status={InvoiceStatusFilter}";
            }

            var invoices = await _apiClient.GetAsync<List<InvoiceListDto>>(query);
            
            if (invoices != null)
            {
                ReceivableDocuments.Clear();
                TotalInvoicesCount = 0;
                DraftInvoicesCount = 0;
                SentInvoicesCount = 0;
                PaidInvoicesCount = 0;
                OverdueInvoicesCount = 0;
                TotalReceivablesOutstanding = 0;
                AmountDueInAgingBucket0to30 = 0;
                AmountDueInAgingBucket31to60 = 0;
                AmountDueInAgingBucket61to90 = 0;
                AmountDueInAgingBucketOver90 = 0;
                TotalCollectionsThisPeriod = 0;
                InvoicesCollectedThisPeriodCount = 0;

                foreach (var invoice in invoices)
                {
                    ReceivableDocuments.Add(invoice);
                    TotalInvoicesCount++;

                    var status = invoice.Status.ToString();
                    if (status == "Draft")
                    {
                        DraftInvoicesCount++;
                    }
                    else if (status == "Sent")
                    {
                        SentInvoicesCount++;
                        TotalReceivablesOutstanding += invoice.AmountDue;

                        var daysOverdue = invoice.DaysOverdue;
                        if (daysOverdue > 0 && daysOverdue <= 30)
                        {
                            AmountDueInAgingBucket0to30 += invoice.AmountDue;
                            OverdueInvoicesCount++;
                        }
                        else if (daysOverdue > 30 && daysOverdue <= 60)
                        {
                            AmountDueInAgingBucket31to60 += invoice.AmountDue;
                            OverdueInvoicesCount++;
                        }
                        else if (daysOverdue > 60 && daysOverdue <= 90)
                        {
                            AmountDueInAgingBucket61to90 += invoice.AmountDue;
                            OverdueInvoicesCount++;
                        }
                        else if (daysOverdue > 90)
                        {
                            AmountDueInAgingBucketOver90 += invoice.AmountDue;
                            OverdueInvoicesCount++;
                        }
                        else if (daysOverdue == 0)
                        {
                            AmountDueInAgingBucket0to30 += invoice.AmountDue;
                        }
                    }
                    else if (status == "Paid")
                    {
                        PaidInvoicesCount++;
                        TotalCollectionsThisPeriod += invoice.AmountPaid;
                        InvoicesCollectedThisPeriodCount++;
                    }
                }

                CalculateCollectionEfficiencyRatio();
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load invoices receivable: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void CreateNewInvoiceEntry()
    {
        // TODO: Open invoice creation dialog with line items
    }

    [RelayCommand]
    private void EditInvoiceEntry(InvoiceListDto? invoice)
    {
        if (invoice == null) return;
        // TODO: Open invoice edit dialog
    }

    [RelayCommand]
    private async Task LoadFullInvoiceDetailsAsync(InvoiceListDto? invoiceSummary)
    {
        if (invoiceSummary == null) return;

        try
        {
            IsBusy = true;
            var fullInvoice = await _apiClient.GetAsync<InvoiceDto>($"api/v1/finance/invoices/{invoiceSummary.Id}");
            SelectedReceivableDocument = fullInvoice;
        }
        catch (Exception ex)
        {
            SetError($"Failed to load invoice details: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SendInvoiceToClientAsync(InvoiceListDto? invoice)
    {
        if (invoice == null) return;

        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>($"api/v1/finance/invoices/{invoice.Id}/send", new { });
            await LoadInvoicesReceivableAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to send invoice: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RecordInvoicePaymentAsync(InvoiceListDto? invoice)
    {
        if (invoice == null) return;
        // TODO: Open payment recording dialog
    }

    [RelayCommand]
    private async Task VoidInvoiceDocumentAsync(InvoiceListDto? invoice)
    {
        if (invoice == null) return;

        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>($"api/v1/finance/invoices/{invoice.Id}/void", new { });
            await LoadInvoicesReceivableAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to void invoice: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task PerformAgingAnalysisAsync()
    {
        try
        {
            IsBusy = true;
            var aging = await _apiClient.GetAsync<dynamic>("api/v1/finance/invoices/aging-analysis");
            
            if (aging != null)
            {
                AmountDueInAgingBucket0to30 = aging.bucket0to30 ?? 0m;
                AmountDueInAgingBucket31to60 = aging.bucket31to60 ?? 0m;
                AmountDueInAgingBucket61to90 = aging.bucket61to90 ?? 0m;
                AmountDueInAgingBucketOver90 = aging.bucketOver90 ?? 0m;
                TotalReceivablesOutstanding = aging.totalOutstanding ?? 0m;
                OverdueInvoicesCount = aging.overdueCount ?? 0;
                CollectionEfficiencyRatio = aging.collectionEfficiency ?? 0m;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to perform aging analysis: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ExportReceivablesAgingAsync()
    {
        try
        {
            IsBusy = true;
            var pdfBytes = await _apiClient.GetBytesAsync("api/v1/finance/invoices/aging-analysis/export");
            
            var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var downloadsFolder = System.IO.Path.Combine(downloadsPath, "Downloads");
            
            if (!System.IO.Directory.Exists(downloadsFolder))
            {
                downloadsFolder = downloadsPath;
            }
            
            var filePath = System.IO.Path.Combine(downloadsFolder, $"AR_Aging_{DateTime.Now:yyyyMMdd}.pdf");
            
            await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            SetError($"Failed to export aging analysis: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GenerateInvoiceReminderBatchAsync()
    {
        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>("api/v1/finance/invoices/send-reminders", new { });
        }
        catch (Exception ex)
        {
            SetError($"Failed to generate reminders: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void CalculateCollectionEfficiencyRatio()
    {
        if (TotalReceivablesOutstanding + TotalCollectionsThisPeriod > 0)
        {
            CollectionEfficiencyRatio = TotalCollectionsThisPeriod / (TotalReceivablesOutstanding + TotalCollectionsThisPeriod) * 100;
        }
        else
        {
            CollectionEfficiencyRatio = 0;
        }
    }

    partial void OnInvoiceDateFilterFromChanged(DateTime value)
    {
        _ = LoadInvoicesReceivableAsync();
    }

    partial void OnInvoiceDateFilterToChanged(DateTime value)
    {
        _ = LoadInvoicesReceivableAsync();
    }

    partial void OnInvoiceStatusFilterChanged(string value)
    {
        _ = LoadInvoicesReceivableAsync();
    }
}
