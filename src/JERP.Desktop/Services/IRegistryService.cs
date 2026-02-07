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

namespace JERP.Desktop.Services;

public interface IRegistryService
{
    string? GetValue(string key, string? defaultValue = null);
    void SetValue(string key, string value);
    void DeleteValue(string key);
    string? GetApiUrl();
    void SetApiUrl(string url);
    string? GetRememberedUsername();
    void SetRememberedUsername(string username);
    void ClearRememberedUsername();
}
