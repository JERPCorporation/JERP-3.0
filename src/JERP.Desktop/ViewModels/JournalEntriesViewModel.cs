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

public partial class JournalEntriesViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private ObservableCollection<JournalEntryDto> _bookEntries = new();

    [ObservableProperty]
    private JournalEntryDto? _selectedBookEntry;

    [ObservableProperty]
    private DateTime _entryDateFrom = DateTime.Now.AddMonths(-1);

    [ObservableProperty]
    private DateTime _entryDateTo = DateTime.Now;

    [ObservableProperty]
    private string _entryStatusFilter = "All";

    [ObservableProperty]
    private int _totalEntriesCount;

    [ObservableProperty]
    private int _draftEntriesCount;

    [ObservableProperty]
    private int _postedEntriesCount;

    [ObservableProperty]
    private int _balancedEntriesCount;

    [ObservableProperty]
    private int _unbalancedEntriesCount;

    [ObservableProperty]
    private decimal _totalDebitsPosted;

    [ObservableProperty]
    private decimal _totalCreditsPosted;

    [ObservableProperty]
    private decimal _netLedgerVariance;

    [ObservableProperty]
    private bool _hasUnbalancedEntries;

    public JournalEntriesViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = LoadJournalEntriesAsync();
    }

    [RelayCommand]
    private async Task LoadJournalEntriesAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var query = $"api/finance/journal-entries?dateFrom={EntryDateFrom:yyyy-MM-dd}&dateTo={EntryDateTo:yyyy-MM-dd}";
            
            if (EntryStatusFilter != "All")
            {
                query += $"&status={EntryStatusFilter}";
            }

            var entries = await _apiClient.GetAsync<List<JournalEntryDto>>(query);
            
            if (entries != null)
            {
                BookEntries.Clear();
                TotalEntriesCount = 0;
                DraftEntriesCount = 0;
                PostedEntriesCount = 0;
                BalancedEntriesCount = 0;
                UnbalancedEntriesCount = 0;
                TotalDebitsPosted = 0;
                TotalCreditsPosted = 0;

                foreach (var entry in entries)
                {
                    BookEntries.Add(entry);
                    TotalEntriesCount++;

                    var status = entry.Status.ToString();
                    if (status == "Draft")
                    {
                        DraftEntriesCount++;
                    }
                    else if (status == "Posted")
                    {
                        PostedEntriesCount++;
                        TotalDebitsPosted += entry.TotalDebit;
                        TotalCreditsPosted += entry.TotalCredit;
                    }

                    if (entry.IsBalanced)
                    {
                        BalancedEntriesCount++;
                    }
                    else
                    {
                        UnbalancedEntriesCount++;
                    }
                }

                NetLedgerVariance = TotalDebitsPosted - TotalCreditsPosted;
                HasUnbalancedEntries = UnbalancedEntriesCount > 0;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load journal entries: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void CreateManualJournalEntry()
    {
        // TODO: Open manual journal entry dialog with line items
    }

    [RelayCommand]
    private void EditJournalEntry(JournalEntryDto? entry)
    {
        if (entry == null || entry.Status.ToString() == "Posted") return;
        // TODO: Open journal entry edit dialog
    }

    [RelayCommand]
    private async Task PostJournalEntryToLedgerAsync(JournalEntryDto? entry)
    {
        if (entry == null || !entry.IsBalanced) return;

        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>($"api/finance/journal-entries/{entry.Id}/post", new { });
            await LoadJournalEntriesAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to post journal entry: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ReversePostedJournalEntryAsync(JournalEntryDto? entry)
    {
        if (entry == null || entry.Status.ToString() != "Posted") return;

        try
        {
            IsBusy = true;
            await _apiClient.PostAsync<object>($"api/finance/journal-entries/{entry.Id}/reverse", new { });
            await LoadJournalEntriesAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to reverse journal entry: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ValidateEntryBalancesAsync()
    {
        try
        {
            IsBusy = true;
            var validation = await _apiClient.PostAsync<dynamic>("api/finance/journal-entries/validate-balances", new { });
            
            if (validation != null && validation.hasIssues == true)
            {
                SetError($"Found {validation.issuesCount} unbalanced entries requiring attention");
            }
            
            await LoadJournalEntriesAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to validate entry balances: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ViewLedgerEntriesAsync(JournalEntryDto? entry)
    {
        if (entry == null) return;
        // TODO: Show ledger entries detail view
    }

    [RelayCommand]
    private async Task FilterByStatusAsync(string status)
    {
        EntryStatusFilter = status;
        await LoadJournalEntriesAsync();
    }

    partial void OnEntryDateFromChanged(DateTime value)
    {
        _ = LoadJournalEntriesAsync();
    }

    partial void OnEntryDateToChanged(DateTime value)
    {
        _ = LoadJournalEntriesAsync();
    }
}
