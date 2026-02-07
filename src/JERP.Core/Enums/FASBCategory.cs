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
/// FASB ASC (Accounting Standards Codification) topic categories
/// </summary>
public enum FASBCategory
{
    /// <summary>
    /// Presentation topics (200s) - Financial statement presentation and format
    /// </summary>
    Presentation = 200,
    
    /// <summary>
    /// Assets topics (300s) - Asset recognition, measurement, and disclosure
    /// </summary>
    Assets = 300,
    
    /// <summary>
    /// Liabilities topics (400s) - Liability recognition, measurement, and disclosure
    /// </summary>
    Liabilities = 400,
    
    /// <summary>
    /// Equity topics (500s) - Equity transactions and reporting
    /// </summary>
    Equity = 500,
    
    /// <summary>
    /// Revenue topics (600s) - Revenue recognition and measurement
    /// </summary>
    Revenue = 600,
    
    /// <summary>
    /// Expenses topics (700s) - Expense recognition and measurement
    /// </summary>
    Expenses = 700,
    
    /// <summary>
    /// Broad Transactions topics (800s) - Cross-cutting accounting issues
    /// </summary>
    BroadTransactions = 800,
    
    /// <summary>
    /// Industry topics (900s) - Industry-specific accounting guidance
    /// </summary>
    Industry = 900
}
