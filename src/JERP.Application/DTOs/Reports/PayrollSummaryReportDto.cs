/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;
using System.Collections.Generic;

namespace JERP.Application.DTOs.Reports;

public class PayrollSummaryReportDto
{
    public Guid CompanyId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public List<PayrollSummaryLineDto> Employees { get; set; } = new();
    
    public decimal TotalGrossWages { get; set; }
    
    public decimal TotalDeductions { get; set; }
    
    public decimal TotalNetPay { get; set; }
    
    public decimal TotalEmployerTaxes { get; set; }
    
    public decimal TotalPayrollCost { get; set; }
    
    // By Department
    public Dictionary<string, decimal> CostByDepartment { get; set; } = new();
    
    // By Pay Type
    public decimal TotalRegularHours { get; set; }
    
    public decimal TotalOvertimeHours { get; set; }
    
    public decimal TotalRegularPay { get; set; }
    
    public decimal TotalOvertimePay { get; set; }
    
    public DateTime GeneratedAt { get; set; }
}
