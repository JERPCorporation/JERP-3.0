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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.DTOs.Auth;
using JERP.Application.Services.Auth;

namespace JERP.Api.Controllers;

/// <summary>
/// Authentication and authorization endpoints.
/// 
/// SECURITY FIX: Logout now calls _authService.RevokeRefreshTokenAsync() to invalidate
/// the user's refresh token. This means even if someone steals the access token, they
/// cannot use the refresh token to get new access tokens after the user logs out.
/// 
/// NOTE ON JWT LOGOUT:
/// JWTs are "stateless" - once issued, they're valid until they expire. You CANNOT truly
/// "invalidate" an access token without a server-side blocklist (which adds complexity).
/// 
/// The practical approach (implemented here):
///   1. Keep access tokens SHORT-LIVED (15-30 minutes) via Jwt:ExpirationMinutes
///   2. On logout, revoke the refresh token so the client can't get new access tokens
///   3. The access token will naturally expire within minutes
/// 
/// If you need IMMEDIATE invalidation (e.g., user reports account compromised), you'd
/// need to add a Redis/in-memory token blocklist. That's a Phase 2 improvement.
/// </summary>
[Route("api/v1/auth")]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Login with username and password
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // SECURITY NOTE: We log the username but NEVER log the password.
        // Structured logging {Username} is safe because Serilog won't accidentally
        // include the full request object.
        _logger.LogInformation("Login attempt for user: {Username}", request.Username);
        
        var result = await _authService.LoginAsync(request);
        
        if (result == null)
        {
            _logger.LogWarning("Failed login attempt for user: {Username}", request.Username);
            return Unauthorized(new { success = false, error = "Invalid username or password" });
        }

        _logger.LogInformation("User {Username} logged in successfully", request.Username);
        return Ok(result);
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        
        if (result == null)
        {
            return Unauthorized(new { success = false, error = "Invalid or expired refresh token" });
        }

        return Ok(result);
    }

    /// <summary>
    /// Get current authenticated user information
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdStr = User.FindFirst("sub")?.Value ?? User.FindFirst("userId")?.Value;
        
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
        {
            return Unauthorized(new { success = false, error = "User not authenticated" });
        }

        var user = await _authService.GetCurrentUserAsync(userId);
        
        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(user);
    }

    /// <summary>
    /// Logout current user - revokes refresh token
    /// </summary>
    /// <remarks>
    /// SECURITY FIX: Previously this endpoint just returned "success" without doing anything.
    /// Now it revokes the refresh token so the client cannot obtain new access tokens.
    /// 
    /// The client should also:
    ///   1. Delete the access token from localStorage/memory
    ///   2. Delete the refresh token from localStorage/memory
    ///   3. Redirect to the login page
    /// </remarks>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst("userId")?.Value 
                  ?? User.FindFirst("sub")?.Value 
                  ?? "unknown";
        
        // Revoke the user's refresh token so they can't get new access tokens
        if (Guid.TryParse(userId, out var parsedUserId))
        {
            await _authService.RevokeRefreshTokenAsync(parsedUserId);
        }

        _logger.LogInformation("User {UserId} logged out - refresh token revoked", userId);
        
        return Ok(new { success = true, message = "Logged out successfully. Please discard your access token." });
    }
}
