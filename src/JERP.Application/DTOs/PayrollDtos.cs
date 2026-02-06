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

// Type aliases to make Payroll DTOs available in the root namespace
// ViewModels import JERP.Application.DTOs and expect these types there

namespace JERP.Application.DTOs;

using PayrollNS = JERP.Application.DTOs.Payroll;

/// <summary>
/// Pay period data transfer object (alias)
/// </summary>
public class PayPeriodDto : PayrollNS.PayPeriodDto
{
}

/// <summary>
/// Payroll record data transfer object (alias)
/// </summary>
public class PayrollRecordDto : PayrollNS.PayrollRecordDto
{
}
