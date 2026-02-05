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
    
    public decimal SubTotal { get; set; }
    
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
}
