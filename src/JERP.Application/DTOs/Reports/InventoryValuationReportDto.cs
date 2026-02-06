/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class InventoryValuationReportDto
{
    public Guid CompanyId { get; set; }
    
    public DateTime AsOfDate { get; set; }
    
    public List<InventoryValuationLineDto> Items { get; set; } = new();
    
    public decimal TotalQuantity { get; set; }
    
    public decimal TotalValue { get; set; }
    
    // By Category
    public Dictionary<string, decimal> ValueByCategory { get; set; } = new();
    
    // By Warehouse
    public Dictionary<string, decimal> ValueByWarehouse { get; set; } = new();
    
    // By Type
    public Dictionary<string, decimal> ValueByType { get; set; } = new();
    
    public DateTime GeneratedAt { get; set; }
}
