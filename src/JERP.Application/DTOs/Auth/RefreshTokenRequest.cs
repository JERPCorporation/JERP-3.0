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

namespace JERP.Application.DTOs.Auth;

/// <summary>
/// Request to refresh an expired access token
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Refresh token issued during login
    /// </summary>
    public required string RefreshToken { get; set; }
}
