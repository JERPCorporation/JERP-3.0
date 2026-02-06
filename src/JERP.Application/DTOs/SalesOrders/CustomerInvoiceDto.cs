using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.SalesOrders;

public class CustomerInvoiceDto
{
    public Guid Id { get; set; }
    
    public string InvoiceNumber { get; set; } = string.Empty;
    
    public Guid CustomerId { get; set; }
    
    public string CustomerName { get; set; } = string.Empty;
    
    public Guid? SalesOrderId { get; set; }
    
    public string? SONumber { get; set; }
    
    public Guid? ShipmentId { get; set; }
    
    public string? ShipmentNumber { get; set; }
    
    public DateTime InvoiceDate { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public string Status { get; set; } = string.Empty; // Draft, Sent, Paid, PartiallyPaid, Overdue, Cancelled
    
    public List<CustomerInvoiceLineDto> LineItems { get; set; } = new();
    
    public decimal Subtotal { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal ShippingAmount { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public decimal AmountPaid { get; set; }
    
    public decimal AmountDue { get; set; }
    
    // Accounting
    public Guid? ARAccountId { get; set; }
    
    public Guid? JournalEntryId { get; set; }
    
    public string? PaymentMethod { get; set; }
    
    public string? PaymentReference { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? PostedAt { get; set; }
    
    public DateTime? PaidAt { get; set; }
    
    // Display Properties
    /// <summary>
    /// Formatted invoice number for display
    /// </summary>
    public string InvoiceNumberDisplay => $"INV-{InvoiceNumber}";
    
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
        "Sent" => "Sent",
        "Paid" => "Paid",
        "PartiallyPaid" => "Partially Paid",
        "Overdue" => "Overdue",
        "Cancelled" => "Cancelled",
        "Void" => "Void",
        _ => Status
    };
    
    /// <summary>
    /// Formatted subtotal for display
    /// </summary>
    public string SubTotalDisplay => SubTotal.ToString("C2");
    
    /// <summary>
    /// Formatted tax amount for display
    /// </summary>
    public string TaxAmountDisplay => TaxAmount.ToString("C2");
    
    /// <summary>
    /// Formatted shipping amount for display
    /// </summary>
    public string ShippingAmountDisplay => ShippingAmount.ToString("C2");
    
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
    /// Indicates if invoice is overdue
    /// </summary>
    public bool IsOverdue => (Status == "Sent" || Status == "PartiallyPaid") && DueDate < DateTime.Now;
    
    /// <summary>
    /// Indicates if invoice is fully paid
    /// </summary>
    public bool IsFullyPaid => Status == "Paid";
    
    /// <summary>
    /// Percentage of invoice paid
    /// </summary>
    public decimal PercentPaid => TotalAmount > 0 ? (AmountPaid / TotalAmount) * 100 : 0;
    
    /// <summary>
    /// Formatted percent paid for display
    /// </summary>
    public string PercentPaidDisplay => $"{PercentPaid:F0}%";
    
    /// <summary>
    /// Status icon for visual representation
    /// </summary>
    public string StatusIcon => Status switch
    {
        "Draft" => "📝",
        "Sent" => "📤",
        "Paid" => "✅",
        "PartiallyPaid" => "⏳",
        "Overdue" => "⚠️",
        "Cancelled" => "❌",
        "Void" => "🚫",
        _ => ""
    };
}
