/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

namespace JERP.Application.DTOs.Finance;

/// <summary>
/// Bill line item DTO
/// </summary>
public class BillLineDto
{
    public Guid Id { get; set; }
    public int LineNumber { get; set; }
    public required string Description { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountName { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
