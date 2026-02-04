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

namespace JERP.Application.DTOs.TaxWithholding;

public class TaxWithholdingDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public int TaxYear { get; set; }
    public string FilingStatus { get; set; } = string.Empty;
    public int FederalAllowances { get; set; }
    public decimal FederalExtraWithholding { get; set; }
    public int StateAllowances { get; set; }
    public decimal StateExtraWithholding { get; set; }
    public bool IsExemptFederal { get; set; }
    public bool IsExemptState { get; set; }
    public DateTime EffectiveDate { get; set; }
}
