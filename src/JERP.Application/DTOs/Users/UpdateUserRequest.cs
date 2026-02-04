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

namespace JERP.Application.DTOs.Users;

/// <summary>
/// Request to update an existing user
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Email address
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Whether the user account is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Role IDs to assign to the user
    /// </summary>
    public List<Guid> RoleIds { get; set; } = new();
}
