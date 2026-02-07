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

using CommunityToolkit.Mvvm.ComponentModel;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels.Finance;

public partial class FinanceViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ChartOfAccountsViewModel _chartOfAccounts;

    [ObservableProperty]
    private JournalEntriesViewModel _journalEntries;

    [ObservableProperty]
    private VendorsViewModel _vendors;

    [ObservableProperty]
    private BillsViewModel _bills;

    [ObservableProperty]
    private CustomersViewModel _customers;

    [ObservableProperty]
    private InvoicesViewModel _invoices;

    public FinanceViewModel(
        IApiClient apiClient,
        IAuthenticationService authService,
        ChartOfAccountsViewModel chartOfAccounts,
        JournalEntriesViewModel journalEntries,
        VendorsViewModel vendors,
        BillsViewModel bills,
        CustomersViewModel customers,
        InvoicesViewModel invoices)
    {
        _apiClient = apiClient;
        _authService = authService;
        _chartOfAccounts = chartOfAccounts;
        _journalEntries = journalEntries;
        _vendors = vendors;
        _bills = bills;
        _customers = customers;
        _invoices = invoices;
    }
}
