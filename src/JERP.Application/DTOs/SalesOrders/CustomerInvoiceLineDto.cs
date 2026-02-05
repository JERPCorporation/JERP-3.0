using System;

namespace JERP.Application.DTOs.SalesOrders;

public class CustomerInvoiceLineDto
{
    public Guid Id { get; set; }
    
    public Guid InvoiceId { get; set; }
    
    public Guid? SalesOrderLineId { get; set; }
    
    public Guid InventoryItemId { get; set; }
    
    public string ItemCode { get; set; } = string.Empty;
    
    public string ItemName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal DiscountAmount { get; set; }
    
    public decimal TaxAmount { get; set; }
    
    public decimal LineTotal { get; set; }
    
    public Guid? RevenueAccountId { get; set; }
}
