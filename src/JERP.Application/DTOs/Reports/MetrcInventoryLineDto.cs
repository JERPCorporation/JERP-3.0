/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;

namespace JERP.Application.DTOs.Reports;

public class MetrcInventoryLineDto
{
    public Guid TransactionId { get; set; }
    
    public DateTime TransactionDate { get; set; }
    
    public string TransactionType { get; set; } = string.Empty; // Receipt, Sale, Transfer, Adjustment
    
    public string PackageTag { get; set; } = string.Empty;
    
    public string ItemName { get; set; } = string.Empty;
    
    public decimal Quantity { get; set; }
    
    public string UnitOfMeasure { get; set; } = string.Empty;
    
    public bool SyncedToMetrc { get; set; }
    
    public DateTime? MetrcSyncDate { get; set; }
    
    public string? MetrcStatus { get; set; }
}
