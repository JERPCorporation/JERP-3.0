using System;

namespace JERP.Application.DTOs.SalesOrders;

public class SalesReturnLineDto
{
    public Guid Id { get; set; }
    
    public Guid SalesReturnId { get; set; }
    
    public Guid? SalesOrderLineId { get; set; }
    
    public Guid InventoryItemId { get; set; }
    
    public string ItemCode { get; set; } = string.Empty;
    
    public string ItemName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal LineTotal { get; set; }
    
    public decimal RestockingFee { get; set; }
    
    public string? Notes { get; set; }
}
