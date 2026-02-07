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
/// Type of physical count
/// </summary>
public enum CountType
{
    /// <summary>
    /// Full physical count of all inventory
    /// </summary>
    Full,
    
    /// <summary>
    /// Cycle count of specific items
    /// </summary>
    Cycle,
    
    /// <summary>
    /// Spot check of random items
    /// </summary>
    Spot
}
