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

using System.ComponentModel.DataAnnotations;

namespace JERP.Core.Entities.Finance;

/// <summary>
/// Represents a vendor for accounts payable
/// </summary>
public class Vendor : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Vendor name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Vendor code/number
    /// </summary>
    [MaxLength(50)]
    public string? VendorCode { get; set; }

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
    /// Address
    /// </summary>
    [MaxLength(500)]
    public string? Address { get; set; }

    /// <summary>
    /// City
    /// </summary>
    [MaxLength(100)]
    public string? City { get; set; }

    /// <summary>
    /// State
    /// </summary>
    [MaxLength(50)]
    public string? State { get; set; }

    /// <summary>
    /// Zip code
    /// </summary>
    [MaxLength(20)]
    public string? ZipCode { get; set; }

    /// <summary>
    /// Tax ID / EIN
    /// </summary>
    [MaxLength(50)]
    public string? TaxId { get; set; }

    /// <summary>
    /// Whether the vendor is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Notes about the vendor
    /// </summary>
    public string? Notes { get; set; }

    // Navigation properties
    public Company Company { get; set; } = null!;
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
