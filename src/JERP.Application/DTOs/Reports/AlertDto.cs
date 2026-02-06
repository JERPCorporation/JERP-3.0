/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 */

using System;

namespace JERP.Application.DTOs.Reports;

public class AlertDto
{
    public Guid AlertId { get; set; }
    
    public string AlertType { get; set; } = string.Empty;
    
    public string Severity { get; set; } = string.Empty; // Critical, Warning, Info
    
    public string Title { get; set; } = string.Empty;
    
    public string Message { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsRead { get; set; }
    
    public string? ActionUrl { get; set; }
}
