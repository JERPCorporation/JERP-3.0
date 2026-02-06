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

using JERP.Application.DTOs;
using JERP.Application.DTOs.Users;

namespace JERP.Desktop.Services;

public interface IAuthenticationService
{
    bool IsAuthenticated { get; }
    UserDto? CurrentUser { get; }
    string? Token { get; }
    
    event EventHandler? AuthenticationStateChanged;
    
    Task<bool> LoginAsync(string username, string password);
    Task LogoutAsync();
    void SaveCredentials(string username);
    string? LoadSavedUsername();
}
