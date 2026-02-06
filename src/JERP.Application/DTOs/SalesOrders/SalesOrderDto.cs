using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.SalesOrders;

public class SalesOrderDto
{
    public Guid Id { get; set; }
    
    public string SONumber { get; set; } = string.Empty;
    
    public Guid CustomerId { get; set; }
    
    public string CustomerName { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty; // Draft, Submitted, Approved, Processing, PartiallyShipped, Shipped, Invoiced, Closed, Cancelled
    
    public DateTime OrderDate { get; set; }
    
    public DateTime? RequestedShipDate { get; set; }
    
    public DateTime? PromisedShipDate { get; set; }
    
    public Guid? WarehouseId { get; set; }
    
    public string? WarehouseName { get; set; }
    
    public string? ShippingMethod { get; set; }
    
    public string? ShippingTerms { get; set; }
    
    public string? PaymentTerms { get; set; }
    
    // Shipping Address
    public string? ShipToAddressLine1 { get; set; }
    
    public string? ShipToAddressLine2 { get; set; }
    
    public string? ShipToCity { get; set; }
    
    public string? ShipToState { get; set; }
    
    public string? ShipToPostalCode { get; set; }
    
    public string? ShipToCountry { get; set; }
    
    // Line Items
    public List<SalesOrderLineDto> LineItems { get; set; } = new();
    
    // Amounts
    public decimal Subtotal { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal ShippingAmount { get; set; }
    
    public decimal DiscountAmount { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    // Fulfillment Tracking
    public decimal TotalShipped { get; set; }
    
    public decimal TotalInvoiced { get; set; }
    
    public bool IsFullyShipped { get; set; }
    
    public bool IsFullyInvoiced { get; set; }
    
    // Approval
    public string? ApprovedBy { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    // Sales Rep
    public Guid? SalesRepId { get; set; }
    
    public string? SalesRepName { get; set; }
    
    // References
    public Guid? SalesQuoteId { get; set; }
    
    public string? SalesQuoteNumber { get; set; }
    
    public string? CustomerPONumber { get; set; }
    
    public string? Notes { get; set; }
    
    public string? InternalNotes { get; set; }
    
    // Cannabis Tracking
    public bool RequiresMetrcTracking { get; set; }
    
    public string? MetrcManifestNumber { get; set; }
    
    // Metadata
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public string? CreatedBy { get; set; }
}
