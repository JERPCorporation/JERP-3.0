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

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Journal entry data transfer object
/// </summary>
public class JournalEntryDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public required string JournalEntryNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public required string Description { get; set; }
    public JournalEntryStatus Status { get; set; }
    public EntrySource Source { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public bool IsBalanced { get; set; }
    public DateTime? PostedAt { get; set; }
    public List<GeneralLedgerEntryDto> LedgerEntries { get; set; } = new();
    public List<JournalEntryLineDto> Lines { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Display Properties
    /// <summary>
    /// Formatted entry number for display
    /// </summary>
    public string EntryNumberDisplay => $"JE-{JournalEntryNumber}";
    
    /// <summary>
    /// Formatted transaction date for display
    /// </summary>
    public string TransactionDateDisplay => TransactionDate.ToString("MMM dd, yyyy");
    
    /// <summary>
    /// Formatted posted date for display
    /// </summary>
    public string PostedDateDisplay => PostedAt.HasValue ? PostedAt.Value.ToString("MMM dd, yyyy") : "Not Posted";
    
    /// <summary>
    /// Friendly status display
    /// </summary>
    public string StatusDisplay => Status switch
    {
        JournalEntryStatus.Draft => "Draft",
        JournalEntryStatus.Posted => "Posted",
        JournalEntryStatus.Voided => "Voided",
        _ => "Unknown"
    };
    
    /// <summary>
    /// Formatted total debits for display
    /// </summary>
    public string TotalDebitDisplay => TotalDebit.ToString("C2");
    
    /// <summary>
    /// Formatted total credits for display
    /// </summary>
    public string TotalCreditDisplay => TotalCredit.ToString("C2");
    
    /// <summary>
    /// Status icon for visual representation
    /// </summary>
    public string StatusIcon => Status switch
    {
        JournalEntryStatus.Posted => "✅",
        JournalEntryStatus.Draft => "📝",
        JournalEntryStatus.Voided => "❌",
        _ => ""
    };
    
    // Computed Properties
    /// <summary>
    /// Balance status indicator
    /// </summary>
    public string BalanceStatus => IsBalanced ? "✅ Balanced" : "⚠️ Out of Balance";
    
    /// <summary>
    /// Difference between debits and credits
    /// </summary>
    public decimal Difference => Math.Abs(TotalDebit - TotalCredit);
    
    /// <summary>
    /// Formatted difference for display
    /// </summary>
    public string DifferenceDisplay => Difference > 0 ? $"Difference: {Difference:C2}" : "";
}

/// <summary>
/// Request to create a manual journal entry
/// </summary>
public class CreateJournalEntryRequest
{
    public Guid CompanyId { get; set; }
    public DateTime TransactionDate { get; set; }
    public required string Description { get; set; }
    public List<CreateLedgerEntryRequest> Entries { get; set; } = new();
}

/// <summary>
/// Request to create a ledger entry line
/// </summary>
public class CreateLedgerEntryRequest
{
    public Guid AccountId { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public required string Description { get; set; }
}
