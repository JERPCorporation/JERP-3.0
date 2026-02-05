using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.SalesOrders;

public class CreateSOShipmentRequest
{
    [Required]
    public Guid SalesOrderId { get; set; }
    
    [Required]
    public DateTime ShipDate { get; set; }
    
    public Guid? WarehouseId { get; set; }
    
    [MaxLength(100)]
    public string? ShippingMethod { get; set; }
    
    [MaxLength(100)]
    public string? TrackingNumber { get; set; }
    
    [MaxLength(100)]
    public string? Carrier { get; set; }
    
    public decimal? ShippingCost { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<CreateSOShipmentLineRequest> LineItems { get; set; } = new();
    
    [MaxLength(100)]
    public string? MetrcManifestNumber { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
}
