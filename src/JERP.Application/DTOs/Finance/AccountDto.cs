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

using JERP.Core.Enums;

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Account data transfer object
/// </summary>
public class AccountDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public required string AccountNumber { get; set; }
    public required string AccountName { get; set; }
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public bool IsSystemAccount { get; set; }
    public bool IsCOGS { get; set; }
    public bool IsNonDeductible { get; set; }
    public string? TaxCategory { get; set; }
    
    // FASB ASC fields (REQUIRED)
    public Guid FASBTopicId { get; set; }
    public Guid FASBSubtopicId { get; set; }
    public string? FASBReference { get; set; }
    public string? FASBTopicName { get; set; }
    public string? FASBSubtopicName { get; set; }
    public FASBCategory? FASBCategory { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Display Properties
    /// <summary>
    /// Display account number (already formatted)
    /// </summary>
    public string AccountNumberDisplay => AccountNumber;
    
    /// <summary>
    /// Friendly account type display
    /// </summary>
    public string AccountTypeDisplay => Type switch
    {
        AccountType.Asset => "Asset",
        AccountType.Liability => "Liability",
        AccountType.Equity => "Equity",
        AccountType.Revenue => "Revenue",
        AccountType.Expense => "Expense",
        _ => "Unknown"
    };
    
    /// <summary>
    /// Active status display
    /// </summary>
    public string IsActiveDisplay => IsActive ? "Active" : "Inactive";
    
    /// <summary>
    /// Formatted balance for display
    /// </summary>
    public string BalanceDisplay => Balance.ToString("C2");
    
    /// <summary>
    /// Status icon for visual representation
    /// </summary>
    public string StatusIcon => IsActive ? "✅" : "❌";
    
    // Computed Properties
    /// <summary>
    /// Indicates if this is a debit balance account
    /// </summary>
    public bool IsDebitBalance => Type == AccountType.Asset || Type == AccountType.Expense;
    
    /// <summary>
    /// Indicates if this is a credit balance account
    /// </summary>
    public bool IsCreditBalance => Type == AccountType.Liability || Type == AccountType.Equity || Type == AccountType.Revenue;
    
    /// <summary>
    /// Full display with number and name
    /// </summary>
    public string FullDisplay => $"{AccountNumber} - {AccountName}";
}

/// <summary>
/// Request to create a new account
/// </summary>
public class CreateAccountRequest
{
    public Guid CompanyId { get; set; }
    public required string AccountNumber { get; set; }
    public required string AccountName { get; set; }
    public AccountType Type { get; set; }
    public bool IsCOGS { get; set; }
    public bool IsNonDeductible { get; set; }
    public string? TaxCategory { get; set; }
    
    // FASB fields (REQUIRED)
    public Guid FASBTopicId { get; set; }
    public Guid FASBSubtopicId { get; set; }
    public string? FASBReference { get; set; }
}

/// <summary>
/// Request to update an existing account
/// </summary>
public class UpdateAccountRequest
{
    public required string AccountName { get; set; }
    public bool IsActive { get; set; }
    public bool IsCOGS { get; set; }
    public bool IsNonDeductible { get; set; }
    public string? TaxCategory { get; set; }
    
    // FASB fields (REQUIRED)
    public Guid FASBTopicId { get; set; }
    public Guid FASBSubtopicId { get; set; }
    public string? FASBReference { get; set; }
}
