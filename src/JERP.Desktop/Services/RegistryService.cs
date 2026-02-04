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

using Microsoft.Win32;

namespace JERP.Desktop.Services;

public class RegistryService : IRegistryService
{
    private const string RegistryPath = @"Software\JERP\Desktop";
    private const string ApiUrlKey = "ApiUrl";
    private const string RememberedUsernameKey = "RememberedUsername";

    public string? GetValue(string key, string? defaultValue = null)
    {
        try
        {
            using var registryKey = Registry.CurrentUser.OpenSubKey(RegistryPath);
            return registryKey?.GetValue(key)?.ToString() ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    public void SetValue(string key, string value)
    {
        try
        {
            using var registryKey = Registry.CurrentUser.CreateSubKey(RegistryPath);
            registryKey.SetValue(key, value);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to write registry value: {key}", ex);
        }
    }

    public void DeleteValue(string key)
    {
        try
        {
            using var registryKey = Registry.CurrentUser.OpenSubKey(RegistryPath, writable: true);
            registryKey?.DeleteValue(key, throwOnMissingValue: false);
        }
        catch
        {
            // Ignore errors when deleting
        }
    }

    public string? GetApiUrl()
    {
        return GetValue(ApiUrlKey);
    }

    public void SetApiUrl(string url)
    {
        SetValue(ApiUrlKey, url);
    }

    public string? GetRememberedUsername()
    {
        return GetValue(RememberedUsernameKey);
    }

    public void SetRememberedUsername(string username)
    {
        SetValue(RememberedUsernameKey, username);
    }

    public void ClearRememberedUsername()
    {
        DeleteValue(RememberedUsernameKey);
    }
}
