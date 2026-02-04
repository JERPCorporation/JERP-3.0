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
    public AccountSubType SubType { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public bool IsSystemAccount { get; set; }
    public bool IsCOGS { get; set; }
    public bool IsNonDeductible { get; set; }
    public string? TaxCategory { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
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
    public AccountSubType SubType { get; set; }
    public bool IsCOGS { get; set; }
    public bool IsNonDeductible { get; set; }
    public string? TaxCategory { get; set; }
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
}
