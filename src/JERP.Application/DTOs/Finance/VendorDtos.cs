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

using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Full vendor details DTO
/// </summary>
public class VendorDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public required string VendorNumber { get; set; }
    public required string CompanyName { get; set; }
    public string? ContactName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? TaxId { get; set; }
    public int PaymentTerms { get; set; }
    public Guid? AccountsPayableAccountId { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public string? CannabisLicense { get; set; }
    public bool IsCannabisVendor { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Display Properties
    /// <summary>
    /// Active status display
    /// </summary>
    public string IsActiveDisplay => IsActive ? "Active" : "Inactive";
    
    /// <summary>
    /// Formatted balance (amount owed to vendor) for display
    /// </summary>
    public string BalanceDisplay => Balance.ToString("C2");
    
    /// <summary>
    /// Status icon for visual representation
    /// </summary>
    public string StatusIcon => IsActive ? "✅" : "❌";
    
    // Computed Properties
    /// <summary>
    /// Indicates if vendor has outstanding balance
    /// </summary>
    public bool HasOutstandingBalance => Balance > 0;
    
    /// <summary>
    /// Warning for high balance vendors
    /// </summary>
    public string BalanceWarning => Balance > 10000 ? "⚠️ High Balance" : "";
}

/// <summary>
/// Vendor list/summary DTO
/// </summary>
public class VendorListDto
{
    public Guid Id { get; set; }
    public required string VendorNumber { get; set; }
    public required string CompanyName { get; set; }
    public string? ContactName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Create vendor DTO
/// </summary>
public class CreateVendorDto
{
    [Required]
    [MaxLength(200)]
    public required string CompanyName { get; set; }
    
    [MaxLength(100)]
    public string? ContactName { get; set; }
    
    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [MaxLength(20)]
    public string? Phone { get; set; }
    
    [MaxLength(200)]
    public string? Street { get; set; }
    
    [MaxLength(100)]
    public string? City { get; set; }
    
    [MaxLength(50)]
    public string? State { get; set; }
    
    [MaxLength(20)]
    public string? PostalCode { get; set; }
    
    [MaxLength(100)]
    public string? Country { get; set; }
    
    [MaxLength(50)]
    public string? TaxId { get; set; }
    
    public int PaymentTerms { get; set; } = 30;
    
    public Guid? AccountsPayableAccountId { get; set; }
    
    [MaxLength(50)]
    public string? CannabisLicense { get; set; }
    
    public bool IsCannabisVendor { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}

/// <summary>
/// Update vendor DTO
/// </summary>
public class UpdateVendorDto
{
    [Required]
    [MaxLength(200)]
    public required string CompanyName { get; set; }
    
    [MaxLength(100)]
    public string? ContactName { get; set; }
    
    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [MaxLength(20)]
    public string? Phone { get; set; }
    
    [MaxLength(200)]
    public string? Street { get; set; }
    
    [MaxLength(100)]
    public string? City { get; set; }
    
    [MaxLength(50)]
    public string? State { get; set; }
    
    [MaxLength(20)]
    public string? PostalCode { get; set; }
    
    [MaxLength(100)]
    public string? Country { get; set; }
    
    [MaxLength(50)]
    public string? TaxId { get; set; }
    
    public int PaymentTerms { get; set; }
    
    public Guid? AccountsPayableAccountId { get; set; }
    
    public bool IsActive { get; set; }
    
    [MaxLength(50)]
    public string? CannabisLicense { get; set; }
    
    public bool IsCannabisVendor { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}
