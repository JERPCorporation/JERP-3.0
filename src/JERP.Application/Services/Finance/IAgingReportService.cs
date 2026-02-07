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

using JERP.Application.DTOs.Finance;

namespace JERP.Application.Services.Finance;

/// <summary>
/// Service interface for aging reports
/// </summary>
public interface IAgingReportService
{
    /// <summary>
    /// Generate accounts payable aging report
    /// </summary>
    Task<APAgingReportDto> GetAPAgingReportAsync(Guid companyId, DateTime asOfDate);
    
    /// <summary>
    /// Generate accounts receivable aging report
    /// </summary>
    Task<ARAgingReportDto> GetARAgingReportAsync(Guid companyId, DateTime asOfDate);
}
