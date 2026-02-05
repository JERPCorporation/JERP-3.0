using System;

namespace JERP.Application.DTOs.SalesOrders;

public class SalesOrderLineDto
{
    public Guid Id { get; set; }
    
    public int LineNumber { get; set; }
    
    public Guid InventoryItemId { get; set; }
    
    public string ItemCode { get; set; } = string.Empty;
    
    public string ItemName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public decimal Quantity { get; set; }
    
    public string UnitOfMeasure { get; set; } = string.Empty;
    
    public decimal UnitPrice { get; set; }
    
    public decimal DiscountPercent { get; set; }
    
    public decimal DiscountAmount { get; set; }
    
    public decimal TaxPercent { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal LineTotal { get; set; }
    
    public decimal QuantityShipped { get; set; }
    
    public decimal QuantityInvoiced { get; set; }
    
    public decimal QuantityRemaining { get; set; }
    
    public bool IsFullyShipped { get; set; }
    
    public bool IsFullyInvoiced { get; set; }
    
    public Guid? RevenueAccountId { get; set; }
    
    public string? Notes { get; set; }
}
