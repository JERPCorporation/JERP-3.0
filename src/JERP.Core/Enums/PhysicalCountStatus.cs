/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: ichbincesartobar@yahoo.com
 */

namespace JERP.Core.Enums;

/// <summary>
/// Status of physical count
/// </summary>
public enum PhysicalCountStatus
{
    /// <summary>
    /// Count is in progress
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Count has been completed
    /// </summary>
    Completed,
    
    /// <summary>
    /// Count has been approved
    /// </summary>
    Approved
}
