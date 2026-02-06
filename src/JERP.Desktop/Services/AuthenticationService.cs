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

using JERP.Application.DTOs;
using JERP.Application.DTOs.Users;

namespace JERP.Desktop.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IApiClient _apiClient;
    private readonly IRegistryService _registryService;

    public bool IsAuthenticated { get; private set; }
    public UserDto? CurrentUser { get; private set; }
    public string? Token { get; private set; }

    public event EventHandler? AuthenticationStateChanged;

    public AuthenticationService(IApiClient apiClient, IRegistryService registryService)
    {
        _apiClient = apiClient;
        _registryService = registryService;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var loginRequest = new { Username = username, Password = password };
            var response = await _apiClient.PostAsync<dynamic>("api/auth/login", loginRequest);
            
            Token = response?.token?.ToString();
            
            if (!string.IsNullOrEmpty(Token))
            {
                _apiClient.SetAuthToken(Token);
                
                var userResponse = await _apiClient.GetAsync<UserDto>("api/users/current");
                CurrentUser = userResponse;
                IsAuthenticated = true;
                
                OnAuthenticationStateChanged();
                return true;
            }
            
            return false;
        }
        catch
        {
            IsAuthenticated = false;
            Token = null;
            CurrentUser = null;
            return false;
        }
    }

    public Task LogoutAsync()
    {
        IsAuthenticated = false;
        Token = null;
        CurrentUser = null;
        _apiClient.SetAuthToken(null);
        
        OnAuthenticationStateChanged();
        return Task.CompletedTask;
    }

    public void SaveCredentials(string username)
    {
        _registryService.SetRememberedUsername(username);
    }

    public string? LoadSavedUsername()
    {
        return _registryService.GetRememberedUsername();
    }

    protected virtual void OnAuthenticationStateChanged()
    {
        AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
    }
}
