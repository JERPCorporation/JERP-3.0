/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;

namespace JERP.Application.DTOs.Reports;

public class PayrollSummaryLineDto
{
    public Guid EmployeeId { get; set; }
    
    public string EmployeeNumber { get; set; } = string.Empty;
    
    public string EmployeeName { get; set; } = string.Empty;
    
    public string Department { get; set; } = string.Empty;
    
    public decimal RegularHours { get; set; }
    
    public decimal OvertimeHours { get; set; }
    
    public decimal GrossWages { get; set; }
    
    public decimal Deductions { get; set; }
    
    public decimal NetPay { get; set; }
    
    public decimal EmployerTaxes { get; set; }
    
    public decimal TotalCost { get; set; }
}
