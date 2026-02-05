using System;
using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.SalesOrders;

public class CreateSalesOrderLineRequest
{
    [Required]
    public Guid InventoryItemId { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Quantity { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }
    
    [Range(0, 100)]
    public decimal DiscountPercent { get; set; }
    
    [Range(0, 100)]
    public decimal TaxPercent { get; set; }
    
    public Guid? RevenueAccountId { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
}
