using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.SalesOrders;

public class SOShipmentDto
{
    public Guid Id { get; set; }
    
    public string ShipmentNumber { get; set; } = string.Empty;
    
    public Guid SalesOrderId { get; set; }
    
    public string SONumber { get; set; } = string.Empty;
    
    public Guid CustomerId { get; set; }
    
    public string CustomerName { get; set; } = string.Empty;
    
    public DateTime ShipDate { get; set; }
    
    public Guid? WarehouseId { get; set; }
    
    public string? WarehouseName { get; set; }
    
    public string? ShippingMethod { get; set; }
    
    public string? TrackingNumber { get; set; }
    
    public string? Carrier { get; set; }
    
    public decimal? ShippingCost { get; set; }
    
    public string Status { get; set; } = string.Empty; // Draft, Picked, Packed, Shipped, Delivered
    
    public List<SOShipmentLineDto> LineItems { get; set; } = new();
    
    // Cannabis Tracking
    public string? MetrcManifestNumber { get; set; }
    
    public DateTime? MetrcManifestDate { get; set; }
    
    public string? PackedBy { get; set; }
    
    public string? ShippedBy { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? ShippedAt { get; set; }
}
