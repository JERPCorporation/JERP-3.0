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

using JERP.Application.DTOs.Auth;
using JERP.Application.DTOs.Users;
using System.Security.Claims;

namespace JERP.Application.Services.Auth;

/// <summary>
/// Interface for authentication and authorization services
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with username and password
    /// </summary>
    Task<TokenResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Refreshes an expired access token
    /// </summary>
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);

    /// <summary>
    /// Gets the current authenticated user information
    /// </summary>
    Task<UserDto> GetCurrentUserAsync(Guid userId);

    /// <summary>
    /// Generates a JWT token for a user
    /// </summary>
    string GenerateJwtToken(Core.Entities.User user);

    /// <summary>
    /// Generates a refresh token
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates a JWT token and returns claims principal
    /// </summary>
    ClaimsPrincipal? ValidateToken(string token);
}
