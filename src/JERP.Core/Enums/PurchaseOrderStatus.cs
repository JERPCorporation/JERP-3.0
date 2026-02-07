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
/// Status of a purchase order
/// </summary>
public enum PurchaseOrderStatus
{
    /// <summary>
    /// Purchase order is being drafted
    /// </summary>
    Draft = 0,
    
    /// <summary>
    /// Purchase order is waiting for approval
    /// </summary>
    PendingApproval = 1,
    
    /// <summary>
    /// Purchase order has been approved
    /// </summary>
    Approved = 2,
    
    /// <summary>
    /// Purchase order has been sent to vendor
    /// </summary>
    Ordered = 3,
    
    /// <summary>
    /// Some items have been received
    /// </summary>
    PartiallyReceived = 4,
    
    /// <summary>
    /// All items have been received
    /// </summary>
    Received = 5,
    
    /// <summary>
    /// Purchase order is closed
    /// </summary>
    Closed = 6,
    
    /// <summary>
    /// Purchase order has been cancelled
    /// </summary>
    Cancelled = 7
}
