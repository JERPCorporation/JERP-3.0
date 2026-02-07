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

namespace JERP.Application.DTOs.PurchaseOrders;

/// <summary>
/// Vendor bill DTO with 3-way matching support
/// </summary>
public class VendorBillDto
{
    public Guid Id { get; set; }
    
    public required string BillNumber { get; set; }
    
    public Guid VendorId { get; set; }
    
    public required string VendorName { get; set; }
    
    public Guid? PurchaseOrderId { get; set; }
    
    public string? PONumber { get; set; }
    
    public required string VendorInvoiceNumber { get; set; }
    
    public DateTime InvoiceDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public required string Status { get; set; }
    
    public List<VendorBillLineDto> LineItems { get; set; } = new();
    
    public decimal Subtotal { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public decimal AmountPaid { get; set; }
    
    public decimal AmountDue { get; set; }
    
    public bool IsPaid { get; set; }
    
    public DateTime? PaymentDate { get; set; }
    
    public Guid? JournalEntryId { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    // Display Properties
    /// <summary>
    /// Formatted bill number for display
    /// </summary>
    public string BillNumberDisplay => $"BILL-{BillNumber}";
    
    /// <summary>
    /// Formatted invoice date for display
    /// </summary>
    public string InvoiceDateDisplay => InvoiceDate.ToString("MMM dd, yyyy");
    
    /// <summary>
    /// Formatted due date for display
    /// </summary>
    public string DueDateDisplay => DueDate.ToString("MMM dd, yyyy");
    
    /// <summary>
    /// Friendly status display
    /// </summary>
    public string StatusDisplay => Status switch
    {
        "Draft" => "Draft",
        "Pending" => "Pending",
        "Approved" => "Approved",
        "Paid" => "Paid",
        "Void" => "Void",
        _ => Status
    };
    
    /// <summary>
    /// Formatted subtotal for display
    /// </summary>
    public string SubTotalDisplay => Subtotal.ToString("C2");
    
    /// <summary>
    /// Formatted tax amount for display
    /// </summary>
    public string TaxAmountDisplay => TaxAmount.ToString("C2");
    
    /// <summary>
    /// Formatted total amount for display
    /// </summary>
    public string TotalAmountDisplay => TotalAmount.ToString("C2");
    
    /// <summary>
    /// Formatted amount paid for display
    /// </summary>
    public string AmountPaidDisplay => AmountPaid.ToString("C2");
    
    /// <summary>
    /// Formatted amount due for display
    /// </summary>
    public string AmountDueDisplay => AmountDue.ToString("C2");
    
    // Computed Properties
    /// <summary>
    /// Indicates if bill is overdue
    /// </summary>
    public bool IsOverdue => !IsPaid && Status != "Void" && DueDate < DateTime.Now;
    
    /// <summary>
    /// Days until due date (negative if overdue)
    /// </summary>
    public int DaysUntilDue => (DueDate - DateTime.Now).Days;
    
    /// <summary>
    /// Urgency indicator for overdue or soon-due bills
    /// </summary>
    public string UrgencyIndicator => IsOverdue ? "⚠️ OVERDUE" : 
                                       DaysUntilDue <= 7 && DaysUntilDue > 0 ? "⏰ Due Soon" : "";
    
    /// <summary>
    /// Status icon for visual representation
    /// </summary>
    public string StatusIcon => Status switch
    {
        "Draft" => "📝",
        "Pending" => "⏳",
        "Approved" => "✅",
        "Paid" => "💰",
        "Void" => "❌",
        _ => ""
    };
}
