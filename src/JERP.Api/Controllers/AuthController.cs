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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JERP.Application.DTOs.Auth;
using JERP.Application.Services.Auth;

namespace JERP.Api.Controllers;

/// <summary>
/// Authentication and authorization endpoints
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
    /// Logout current user
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        var userId = User.FindFirst("userId")?.Value ?? "0";
        
        _logger.LogInformation("User {UserId} logged out", userId);
        
        return Ok(new { success = true, message = "Logged out successfully" });
    }
}
