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

using System.ComponentModel.DataAnnotations;

namespace JERP.Core.Entities;

/// <summary>
/// User roles in the system
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Super administrator with full system access
    /// </summary>
    SuperAdmin = 0,

    /// <summary>
    /// Company administrator
    /// </summary>
    Admin = 1,

    /// <summary>
    /// Payroll manager with payroll-specific permissions
    /// </summary>
    PayrollManager = 2,

    /// <summary>
    /// HR manager with employee management permissions
    /// </summary>
    HRManager = 3,

    /// <summary>
    /// Accountant with financial permissions
    /// </summary>
    Accountant = 4,

    /// <summary>
    /// Regular employee with limited access
    /// </summary>
    Employee = 5
}

/// <summary>
/// User account status
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// Account is active and can be used
    /// </summary>
    Active = 0,

    /// <summary>
    /// Account is temporarily suspended
    /// </summary>
    Suspended = 1,

    /// <summary>
    /// Account is inactive
    /// </summary>
    Inactive = 2
}

/// <summary>
/// Represents a system user
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Unique username for authentication
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password for authentication
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// User's first name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the user account is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// User's role in the system
    /// </summary>
    [Required]
    public UserRole Role { get; set; } = UserRole.Employee;

    /// <summary>
    /// Current status of the user account
    /// </summary>
    [Required]
    public UserStatus Status { get; set; } = UserStatus.Active;

    /// <summary>
    /// Timestamp of the user's last login
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// IP address of the user's last login
    /// </summary>
    [MaxLength(50)]
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// Number of consecutive failed login attempts
    /// </summary>
    public int FailedLoginAttempts { get; set; } = 0;

    /// <summary>
    /// Timestamp until which the account is locked out (null if not locked)
    /// </summary>
    public DateTime? LockoutUntil { get; set; }

    // Navigation properties
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
