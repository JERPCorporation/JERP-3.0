using System;

namespace JERP.Application.DTOs.SalesOrders;

public class SOShipmentLineDto
{
    public Guid Id { get; set; }
    
    public Guid SalesOrderLineId { get; set; }
    
    public Guid InventoryItemId { get; set; }
    
    public string ItemCode { get; set; } = string.Empty;
    
    public string ItemName { get; set; } = string.Empty;
    
    public decimal QuantityOrdered { get; set; }
    
    public decimal QuantityShipped { get; set; }
    
    public decimal QuantityPreviouslyShipped { get; set; }
    
    public string UnitOfMeasure { get; set; } = string.Empty;
    
    public Guid? BatchLotId { get; set; }
    
    public string? BatchNumber { get; set; }
    
    public string? SerialNumber { get; set; }
    
    public string? BinLocation { get; set; }
    
    public string? Notes { get; set; }
}
