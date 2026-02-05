using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.SalesOrders;

public class CreateSalesOrderRequest
{
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public DateTime OrderDate { get; set; }
    
    public DateTime? RequestedShipDate { get; set; }
    
    public Guid? WarehouseId { get; set; }
    
    [MaxLength(100)]
    public string? ShippingMethod { get; set; }
    
    [MaxLength(50)]
    public string? ShippingTerms { get; set; }
    
    [MaxLength(50)]
    public string? PaymentTerms { get; set; }
    
    // Shipping Address (override customer default)
    [MaxLength(200)]
    public string? ShipToAddressLine1 { get; set; }
    
    [MaxLength(200)]
    public string? ShipToAddressLine2 { get; set; }
    
    [MaxLength(100)]
    public string? ShipToCity { get; set; }
    
    [MaxLength(50)]
    public string? ShipToState { get; set; }
    
    [MaxLength(20)]
    public string? ShipToPostalCode { get; set; }
    
    [MaxLength(100)]
    public string? ShipToCountry { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateSalesOrderLineRequest> LineItems { get; set; } = new();
    
    public decimal ShippingAmount { get; set; }
    
    public decimal DiscountAmount { get; set; }
    
    public Guid? SalesRepId { get; set; }
    
    public Guid? SalesQuoteId { get; set; }
    
    [MaxLength(50)]
    public string? CustomerPONumber { get; set; }
    
    [MaxLength(2000)]
    public string? Notes { get; set; }
    
    [MaxLength(2000)]
    public string? InternalNotes { get; set; }
}
