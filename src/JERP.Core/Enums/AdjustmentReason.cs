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
/// Reason for inventory adjustment
/// </summary>
public enum AdjustmentReason
{
    /// <summary>
    /// Inventory shrinkage
    /// </summary>
    Shrinkage,
    
    /// <summary>
    /// Damaged goods
    /// </summary>
    Damage,
    
    /// <summary>
    /// Expired products
    /// </summary>
    Expired,
    
    /// <summary>
    /// Found inventory
    /// </summary>
    Found,
    
    /// <summary>
    /// Theft
    /// </summary>
    Theft,
    
    /// <summary>
    /// Physical count variance adjustment
    /// </summary>
    PhysicalCountVariance,
    
    /// <summary>
    /// Write off
    /// </summary>
    WriteOff,
    
    /// <summary>
    /// Returned goods
    /// </summary>
    Returned,
    
    /// <summary>
    /// Other reason
    /// </summary>
    Other
}
