/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

namespace JERP.Core.Enums;

/// <summary>
/// Represents the source system that created a journal/GL entry
/// </summary>
public enum EntrySource
{
    Manual,
    Payroll,
    Invoice,
    Bill,
    Payment,
    BankReconciliation,
    YearEnd,
    Other
}
