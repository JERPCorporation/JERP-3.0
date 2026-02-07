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
/// Status of stock transfer
/// </summary>
public enum TransferStatus
{
    /// <summary>
    /// Transfer is being drafted
    /// </summary>
    Draft,
    
    /// <summary>
    /// Transfer is in transit
    /// </summary>
    InTransit,
    
    /// <summary>
    /// Transfer has been completed
    /// </summary>
    Completed
}
