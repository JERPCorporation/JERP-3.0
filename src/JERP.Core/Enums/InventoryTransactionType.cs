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
/// Type of inventory transaction
/// </summary>
public enum InventoryTransactionType
{
    /// <summary>
    /// Stock receipt from purchase order
    /// </summary>
    Receipt,
    
    /// <summary>
    /// Inventory adjustment
    /// </summary>
    Adjustment,
    
    /// <summary>
    /// Sale to customer
    /// </summary>
    Sale,
    
    /// <summary>
    /// Stock transfer in from another warehouse
    /// </summary>
    TransferIn,
    
    /// <summary>
    /// Stock transfer out to another warehouse
    /// </summary>
    TransferOut,
    
    /// <summary>
    /// Physical count adjustment
    /// </summary>
    CountAdjustment,
    
    /// <summary>
    /// Customer return
    /// </summary>
    Return
}
