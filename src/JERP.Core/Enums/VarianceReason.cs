/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

namespace JERP.Core.Enums;

/// <summary>
/// Reason for physical count variance
/// </summary>
public enum VarianceReason
{
    /// <summary>
    /// Inventory shrinkage
    /// </summary>
    Shrinkage,
    
    /// <summary>
    /// Counting error
    /// </summary>
    CountError,
    
    /// <summary>
    /// Theft
    /// </summary>
    Theft,
    
    /// <summary>
    /// Damaged goods
    /// </summary>
    Damage,
    
    /// <summary>
    /// System error
    /// </summary>
    SystemError,
    
    /// <summary>
    /// Other reason
    /// </summary>
    Other
}
