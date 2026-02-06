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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a vendor/supplier in the accounts payable system
/// </summary>
public class Vendor : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// Vendor code/number
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string VendorNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Vendor company name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string CompanyName { get; set; } = string.Empty;
    
    /// <summary>
    /// Contact person name
    /// </summary>
    [MaxLength(100)]
    public string? ContactName { get; set; }
    
    /// <summary>
    /// Contact email
    /// </summary>
    [MaxLength(100)]
    public string? Email { get; set; }
    
    /// <summary>
    /// Contact phone
    /// </summary>
    [MaxLength(20)]
    public string? Phone { get; set; }
    
    /// <summary>
    /// Street address
    /// </summary>
    [MaxLength(200)]
    public string? Street { get; set; }
    
    /// <summary>
    /// City
    /// </summary>
    [MaxLength(100)]
    public string? City { get; set; }
    
    /// <summary>
    /// State/Province
    /// </summary>
    [MaxLength(50)]
    public string? State { get; set; }
    
    /// <summary>
    /// Postal/ZIP code
    /// </summary>
    [MaxLength(20)]
    public string? PostalCode { get; set; }
    
    /// <summary>
    /// Country
    /// </summary>
    [MaxLength(100)]
    public string? Country { get; set; }
    
    /// <summary>
    /// Tax ID/EIN
    /// </summary>
    [MaxLength(50)]
    public string? TaxId { get; set; }
    
    /// <summary>
    /// Payment terms in days
    /// </summary>
    public int PaymentTerms { get; set; } = 30;
    
    /// <summary>
    /// Accounts payable account
    /// </summary>
    public Guid? AccountsPayableAccountId { get; set; }
    
    /// <summary>
    /// Current balance owed to vendor
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }
    
    /// <summary>
    /// Whether vendor is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Cannabis license number
    /// </summary>
    [MaxLength(50)]
    public string? CannabisLicense { get; set; }
    
    /// <summary>
    /// Is cannabis vendor
    /// </summary>
    public bool IsCannabisVendor { get; set; }
    
    /// <summary>
    /// Notes about the vendor
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    // Navigation properties
    public Company Company { get; set; } = null!;
    public Account? AccountsPayableAccount { get; set; }
    public ICollection<VendorBill> Bills { get; set; } = new List<VendorBill>();
}
