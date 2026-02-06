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

public partial class BillsViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<BillListDto> _payableDocuments = new();

    [ObservableProperty]
    private BillDto? _selectedPayableDocument;

    [ObservableProperty]
    private DateTime _billDateFilterFrom = DateTime.Now.AddMonths(-3);

    [ObservableProperty]
    private DateTime _billDateFilterTo = DateTime.Now.AddMonths(1);

    [ObservableProperty]
    private string _billStatusFilter = "All";

    [ObservableProperty]
    private int _totalBillsCount;

    [ObservableProperty]
    private int _draftBillsCount;

    [ObservableProperty]
    private int _unpaidBillsCount;

    [ObservableProperty]
    private int _paidBillsCount;

    [ObservableProperty]
    private int _overdueBillsCount;

    [ObservableProperty]
    private decimal _totalPayablesOutstanding;

    [ObservableProperty]
    private decimal _amountDueInAgingBucket0to30;

    [ObservableProperty]
    private decimal _amountDueInAgingBucket31to60;

    [ObservableProperty]
    private decimal _amountDueInAgingBucket61to90;

    [ObservableProperty]
    private decimal _amountDueInAgingBucketOver90;

    [ObservableProperty]
    private decimal _totalPaymentsThisPeriod;

    [ObservableProperty]
    private int _billsPaidThisPeriodCount;

    public BillsViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadBillsPayableAsync();
    }

    [RelayCommand]
    private async Task LoadBillsPayableAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/v1/vendors/bills?dateFrom={BillDateFilterFrom:yyyy-MM-dd}&dateTo={BillDateFilterTo:yyyy-MM-dd}";
            
            if (BillStatusFilter != "All")
            {
                query += $"&status={BillStatusFilter}";
            }

            var bills = await _apiClient.GetAsync<List<BillListDto>>(query);
            
            if (bills != null)
            {
                PayableDocuments.Clear();
                TotalBillsCount = 0;
                DraftBillsCount = 0;
                UnpaidBillsCount = 0;
                PaidBillsCount = 0;
                OverdueBillsCount = 0;
                TotalPayablesOutstanding = 0;
                AmountDueInAgingBucket0to30 = 0;
                AmountDueInAgingBucket31to60 = 0;
                AmountDueInAgingBucket61to90 = 0;
                AmountDueInAgingBucketOver90 = 0;
                TotalPaymentsThisPeriod = 0;
                BillsPaidThisPeriodCount = 0;

                foreach (var bill in bills)
                {
                    PayableDocuments.Add(bill);
                    TotalBillsCount++;

                    var status = bill.Status.ToString();
                    if (status == "Draft")
                    {
                        DraftBillsCount++;
                    }
                    else if (status == "Unpaid")
                    {
                        UnpaidBillsCount++;
                        TotalPayablesOutstanding += bill.AmountDue;

                        var daysOverdue = bill.DaysOverdue;
                        if (daysOverdue > 0 && daysOverdue <= 30)
                        {
                            AmountDueInAgingBucket0to30 += bill.AmountDue;
                            OverdueBillsCount++;
                        }
                        else if (daysOverdue > 30 && daysOverdue <= 60)
                        {
                            AmountDueInAgingBucket31to60 += bill.AmountDue;
                            OverdueBillsCount++;
                        }
                        else if (daysOverdue > 60 && daysOverdue <= 90)
                        {
                            AmountDueInAgingBucket61to90 += bill.AmountDue;
                            OverdueBillsCount++;
                        }
                        else if (daysOverdue > 90)
                        {
                            AmountDueInAgingBucketOver90 += bill.AmountDue;
                            OverdueBillsCount++;
                        }
                        else if (daysOverdue == 0)
                        {
                            AmountDueInAgingBucket0to30 += bill.AmountDue;
                        }
                    }
                    else if (status == "Paid")
                    {
                        PaidBillsCount++;
                        TotalPaymentsThisPeriod += bill.AmountPaid;
                        BillsPaidThisPeriodCount++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load bills payable: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void CreateNewBillEntry()
    {
        // TODO: Open bill creation dialog with line items
    }

    [RelayCommand]
    private void EditBillEntry(BillListDto? bill)
    {
        if (bill == null) return;
        // TODO: Open bill edit dialog
    }

    [RelayCommand]
    private async Task LoadFullBillDetailsAsync(BillListDto? billSummary)
    {
        if (billSummary == null) return;

        try
        {
            IsBusy = true;
            var fullBill = await _apiClient.GetAsync<BillDto>($"api/v1/vendors/bills/{billSummary.Id}");
            SelectedPayableDocument = fullBill;
        }
        catch (Exception ex)
        {
            SetError($"Failed to load bill details: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RecordBillPaymentAsync(BillListDto? bill)
    {
        if (bill == null) return;
        // TODO: Open payment recording dialog
    }

    [RelayCommand]
    private async Task VoidBillDocumentAsync(BillListDto? bill)
    {
        if (bill == null) return;

        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>($"api/v1/vendors/bills/{bill.Id}/void", new { });
            await LoadBillsPayableAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to void bill: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CalculateAgingBucketsAsync()
    {
        try
        {
            IsBusy = true;
            var aging = await _apiClient.GetAsync<dynamic>("api/v1/vendors/bills/aging-report");
            
            if (aging != null)
            {
                AmountDueInAgingBucket0to30 = aging.bucket0to30 ?? 0m;
                AmountDueInAgingBucket31to60 = aging.bucket31to60 ?? 0m;
                AmountDueInAgingBucket61to90 = aging.bucket61to90 ?? 0m;
                AmountDueInAgingBucketOver90 = aging.bucketOver90 ?? 0m;
                TotalPayablesOutstanding = aging.totalOutstanding ?? 0m;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to calculate aging buckets: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ExportPayablesAgingAsync()
    {
        try
        {
            IsBusy = true;
            var pdfBytes = await _apiClient.GetBytesAsync("api/v1/vendors/bills/aging-report/export");
            
            var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var downloadsFolder = System.IO.Path.Combine(downloadsPath, "Downloads");
            
            if (!System.IO.Directory.Exists(downloadsFolder))
            {
                downloadsFolder = downloadsPath;
            }
            
            var filePath = System.IO.Path.Combine(downloadsFolder, $"AP_Aging_{DateTime.Now:yyyyMMdd}.pdf");
            
            await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            SetError($"Failed to export aging report: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnBillDateFilterFromChanged(DateTime value)
    {
        _ = LoadBillsPayableAsync();
    }

    partial void OnBillDateFilterToChanged(DateTime value)
    {
        _ = LoadBillsPayableAsync();
    }

    partial void OnBillStatusFilterChanged(string value)
    {
        _ = LoadBillsPayableAsync();
    }
}
