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
using System.ComponentModel.DataAnnotations.Schema;
using JERP.Core.Entities.Finance;
using JERP.Core.Enums;

namespace JERP.Core.Entities.SalesOrders;

/// <summary>
/// Represents a sales return (RMA)
/// </summary>
public class SalesReturn : BaseEntity
{
    /// <summary>
    /// Foreign key to the company
    /// </summary>
    [Required]
    public Guid CompanyId { get; set; }
    
    /// <summary>
    /// RMA number (auto-generated: RMA-0001)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string RMANumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Foreign key to customer
    /// </summary>
    [Required]
    public Guid CustomerId { get; set; }
    
    /// <summary>
    /// Foreign key to sales order
    /// </summary>
    public Guid? SalesOrderId { get; set; }
    
    /// <summary>
    /// Foreign key to invoice
    /// </summary>
    public Guid? InvoiceId { get; set; }
    
    /// <summary>
    /// Return date
    /// </summary>
    public DateTime ReturnDate { get; set; }
    
    /// <summary>
    /// Return status
    /// </summary>
    public SalesReturnStatus Status { get; set; } = SalesReturnStatus.Requested;
    
    /// <summary>
    /// Reason for return
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Reason { get; set; } = string.Empty;
    
    /// <summary>
    /// Return type
    /// </summary>
    [MaxLength(50)]
    public string? ReturnType { get; set; }
    
    // Amounts
    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    
    // Approval
    [MaxLength(100)]
    public string? ApprovedBy { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    [MaxLength(100)]
    public string? ReceivedBy { get; set; }
    
    public DateTime? ReceivedAt { get; set; }
    
    [MaxLength(2000)]
    public string? Notes { get; set; }
    
    // Navigation properties
    public Company Company { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public SalesOrder? SalesOrder { get; set; }
    public CustomerInvoice? Invoice { get; set; }
    public ICollection<SalesReturnLine> LineItems { get; set; } = new List<SalesReturnLine>();
}
