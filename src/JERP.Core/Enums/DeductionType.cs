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
/// Represents the type of payroll deduction
/// </summary>
public enum DeductionType
{
    Health,
    Dental,
    Vision,
    Life,
    Retirement401k,
    RetirementIRA,
    HSA,
    FSA,
    Garnishment,
    ChildSupport,
    Other
}
