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
/// Status of inventory adjustment
/// </summary>
public enum AdjustmentStatus
{
    /// <summary>
    /// Adjustment is being drafted
    /// </summary>
    Draft,
    
    /// <summary>
    /// Adjustment has been approved
    /// </summary>
    Approved,
    
    /// <summary>
    /// Adjustment has been posted to general ledger
    /// </summary>
    Posted
}
