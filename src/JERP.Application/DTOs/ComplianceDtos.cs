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

// Type alias to make Compliance DTOs available in the root namespace
// ViewModels import JERP.Application.DTOs and expect these types there

namespace JERP.Application.DTOs;

using ComplianceNS = JERP.Application.DTOs.Compliance;

/// <summary>
/// Compliance violation data transfer object (alias)
/// </summary>
public class ComplianceViolationDto : ComplianceNS.ComplianceViolationDto
{
}
