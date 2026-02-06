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

public partial class JournalEntriesViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<JournalEntryDto> _entryCollection = new();

    [ObservableProperty]
    private JournalEntryDto? _currentlySelectedEntry;

    [ObservableProperty]
    private string _lookupText = string.Empty;

    [ObservableProperty]
    private JournalEntryStatus? _statusFilterCriteria;

    [ObservableProperty]
    private int _currentPageNumber = 1;

    [ObservableProperty]
    private int _maxPageNumber = 1;

    [ObservableProperty]
    private int _itemsPerPage = 25;

    public JournalEntriesViewModel(IApiClient apiClient, IAuthenticationService authService)
    {
        _apiClient = apiClient;
        _authService = authService;
        _ = RetrieveJournalEntriesAsync();
    }

    [RelayCommand]
    private async Task RetrieveJournalEntriesAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var organizationId = _authService.CurrentUser?.CompanyId;
            if (organizationId == null)
            {
                SetError("Organization context unavailable");
                return;
            }

            var endpointUrl = $"api/finance/journal-entries?companyId={organizationId}&page={CurrentPageNumber}&pageSize={ItemsPerPage}";
            
            if (!string.IsNullOrWhiteSpace(LookupText))
            {
                endpointUrl += $"&search={Uri.EscapeDataString(LookupText)}";
            }

            if (StatusFilterCriteria.HasValue)
            {
                endpointUrl += $"&status={StatusFilterCriteria.Value}";
            }

            var responseData = await _apiClient.GetAsync<dynamic>(endpointUrl);
            
            if (responseData != null)
            {
                EntryCollection.Clear();
                
                var recordSet = responseData.items as IEnumerable<dynamic>;
                if (recordSet != null)
                {
                    foreach (var record in recordSet)
                    {
                        var journalEntry = new JournalEntryDto
                        {
                            Id = record.id,
                            CompanyId = record.companyId,
                            JournalEntryNumber = record.journalEntryNumber,
                            TransactionDate = record.transactionDate,
                            Description = record.description,
                            Status = (JournalEntryStatus)record.status,
                            Source = (EntrySource)record.source,
                            TotalDebit = record.totalDebit,
                            TotalCredit = record.totalCredit,
                            IsBalanced = record.isBalanced,
                            PostedAt = record.postedAt,
                            CreatedAt = record.createdAt,
                            UpdatedAt = record.updatedAt
                        };

                        if (record.ledgerEntries != null)
                        {
                            foreach (var ledgerEntry in record.ledgerEntries)
                            {
                                journalEntry.LedgerEntries.Add(new GeneralLedgerEntryDto
                                {
                                    Id = ledgerEntry.id,
                                    CompanyId = ledgerEntry.companyId,
                                    JournalEntryId = ledgerEntry.journalEntryId,
                                    AccountId = ledgerEntry.accountId,
                                    AccountNumber = ledgerEntry.accountNumber,
                                    AccountName = ledgerEntry.accountName,
                                    TransactionDate = ledgerEntry.transactionDate,
                                    DebitAmount = ledgerEntry.debitAmount,
                                    CreditAmount = ledgerEntry.creditAmount,
                                    Description = ledgerEntry.description,
                                    Source = (EntrySource)ledgerEntry.source,
                                    SourceEntityId = ledgerEntry.sourceEntityId,
                                    Is280EDeductible = ledgerEntry.is280EDeductible,
                                    CreatedAt = ledgerEntry.createdAt
                                });
                            }
                        }

                        EntryCollection.Add(journalEntry);
                    }
                }
                
                MaxPageNumber = responseData.totalPages ?? 1;
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to retrieve journal entries: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task PerformSearchAsync()
    {
        CurrentPageNumber = 1;
        await RetrieveJournalEntriesAsync();
    }

    [RelayCommand]
    private async Task FilterByStatusAsync(JournalEntryStatus? entryStatus)
    {
        StatusFilterCriteria = entryStatus;
        CurrentPageNumber = 1;
        await RetrieveJournalEntriesAsync();
    }

    [RelayCommand]
    private void CreateNewEntry()
    {
        // TODO: Launch journal entry creation dialog
    }

    [RelayCommand]
    private void EditExistingEntry(JournalEntryDto? entryRecord)
    {
        if (entryRecord == null) return;
        // TODO: Launch journal entry editing dialog
    }

    [RelayCommand]
    private async Task PublishEntryToLedgerAsync(JournalEntryDto? entryRecord)
    {
        if (entryRecord == null) return;

        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var organizationId = _authService.CurrentUser?.CompanyId;
            if (organizationId == null)
            {
                SetError("Organization context unavailable");
                return;
            }

            await _apiClient.PostAsync<object>($"api/finance/journal-entries/{entryRecord.Id}/post", null);
            await RetrieveJournalEntriesAsync();
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
    private async Task InvalidateEntryAsync(JournalEntryDto? entryRecord)
    {
        if (entryRecord == null) return;

        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var organizationId = _authService.CurrentUser?.CompanyId;
            if (organizationId == null)
            {
                SetError("Organization context unavailable");
                return;
            }

            await _apiClient.PostAsync<object>($"api/finance/journal-entries/{entryRecord.Id}/void", null);
            await RetrieveJournalEntriesAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to void journal entry: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteEntryRecordAsync(JournalEntryDto? entryRecord)
    {
        if (entryRecord == null) return;

        try
        {
            var organizationId = _authService.CurrentUser?.CompanyId;
            if (organizationId == null)
            {
                SetError("Organization context unavailable");
                return;
            }

            await _apiClient.DeleteAsync($"api/finance/journal-entries/{entryRecord.Id}");
            await RetrieveJournalEntriesAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to delete journal entry: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task NavigateForwardAsync()
    {
        if (CurrentPageNumber < MaxPageNumber)
        {
            CurrentPageNumber++;
            await RetrieveJournalEntriesAsync();
        }
    }

    [RelayCommand]
    private async Task NavigateBackwardAsync()
    {
        if (CurrentPageNumber > 1)
        {
            CurrentPageNumber--;
            await RetrieveJournalEntriesAsync();
        }
    }
}
