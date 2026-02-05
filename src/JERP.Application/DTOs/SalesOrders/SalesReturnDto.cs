using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.SalesOrders;

public class SalesReturnDto
{
    public Guid Id { get; set; }
    
    public string RMANumber { get; set; } = string.Empty;
    
    public Guid CustomerId { get; set; }
    
    public string CustomerName { get; set; } = string.Empty;
    
    public Guid? SalesOrderId { get; set; }
    
    public string? SONumber { get; set; }
    
    public Guid? InvoiceId { get; set; }
    
    public string? InvoiceNumber { get; set; }
    
    public DateTime ReturnDate { get; set; }
    
    public string Status { get; set; } = string.Empty; // Requested, Approved, Received, Refunded, Closed
    
    public string Reason { get; set; } = string.Empty; // Defective, Wrong Item, Customer Changed Mind, Damaged
    
    public string? ReturnType { get; set; } // Refund, Credit, Exchange
    
    public List<SalesReturnLineDto> LineItems { get; set; } = new();
    
    public decimal SubTotal { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public string? ApprovedBy { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    public string? ReceivedBy { get; set; }
    
    public DateTime? ReceivedAt { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
