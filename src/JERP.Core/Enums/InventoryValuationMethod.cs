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
/// Inventory valuation method for COGS calculation
/// </summary>
public enum InventoryValuationMethod
{
    /// <summary>
    /// First In, First Out - oldest inventory sold first
    /// </summary>
    FIFO = 0,
    
    /// <summary>
    /// Weighted Average Cost - average cost across all inventory
    /// </summary>
    WeightedAverage = 1,
    
    /// <summary>
    /// Last In, First Out - newest inventory sold first (GAAP only, not IFRS)
    /// </summary>
    LIFO = 2
}
