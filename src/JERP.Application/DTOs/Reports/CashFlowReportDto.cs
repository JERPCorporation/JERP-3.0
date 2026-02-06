/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class CashFlowReportDto
{
    public Guid CompanyId { get; set; }
    
    public string CompanyName { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public decimal BeginningCashBalance { get; set; }
    
    // Operating Activities
    public List<ReportLineItemDto> OperatingActivities { get; set; } = new();
    
    public decimal NetCashFromOperating { get; set; }
    
    // Investing Activities
    public List<ReportLineItemDto> InvestingActivities { get; set; } = new();
    
    public decimal NetCashFromInvesting { get; set; }
    
    // Financing Activities
    public List<ReportLineItemDto> FinancingActivities { get; set; } = new();
    
    public decimal NetCashFromFinancing { get; set; }
    
    public decimal NetCashChange { get; set; }
    
    public decimal EndingCashBalance { get; set; }
    
    public DateTime GeneratedAt { get; set; }
}
