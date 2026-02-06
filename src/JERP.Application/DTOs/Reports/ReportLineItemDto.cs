/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;

namespace JERP.Application.DTOs.Reports;

public class ReportLineItemDto
{
    public Guid? AccountId { get; set; }
    
    public string AccountNumber { get; set; } = string.Empty;
    
    public string AccountName { get; set; } = string.Empty;
    
    public string? CategoryName { get; set; }
    
    public decimal CurrentAmount { get; set; }
    
    public decimal? PriorAmount { get; set; }
    
    public decimal? BudgetAmount { get; set; }
    
    public decimal? Variance { get; set; }
    
    public decimal? VariancePercent { get; set; }
    
    public int Level { get; set; } // For hierarchical display
    
    public bool IsSubtotal { get; set; }
    
    public bool IsBold { get; set; }
}
