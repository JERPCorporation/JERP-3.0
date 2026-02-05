using System;
using System.ComponentModel.DataAnnotations;

namespace JERP.Application.DTOs.SalesOrders;

public class CreateSOShipmentLineRequest
{
    [Required]
    public Guid SalesOrderLineId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal QuantityShipped { get; set; }
    
    public Guid? BatchLotId { get; set; }
    
    [MaxLength(50)]
    public string? SerialNumber { get; set; }
    
    [MaxLength(50)]
    public string? BinLocation { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
}
