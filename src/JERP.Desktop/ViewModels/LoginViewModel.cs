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

using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JERP.Desktop.Services;
using JERP.Desktop.Views;

namespace JERP.Desktop.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _rememberMe;

    [ObservableProperty]
    private bool _isLoading;

    public LoginViewModel(IAuthenticationService authService, IServiceProvider serviceProvider)
    {
        _authService = authService;
        _serviceProvider = serviceProvider;
        
        var savedUsername = _authService.LoadSavedUsername();
        if (!string.IsNullOrEmpty(savedUsername))
        {
            Username = savedUsername;
            RememberMe = true;
        }
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = null;
        
        if (string.IsNullOrWhiteSpace(Username))
        {
            SetError("Please enter a username");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            SetError("Please enter a password");
            return;
        }

        IsLoading = true;
        IsBusy = true;

        try
        {
            var success = await _authService.LoginAsync(Username, Password);
            
            if (success)
            {
                if (RememberMe)
                {
                    _authService.SaveCredentials(Username);
                }

                var mainWindow = _serviceProvider.GetService(typeof(MainWindow)) as MainWindow;
                mainWindow?.Show();
                
                System.Windows.Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault()?.Close();
            }
            else
            {
                SetError("Invalid username or password");
            }
        }
        catch (Exception ex)
        {
            SetError($"Login failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            IsBusy = false;
            Password = string.Empty;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        System.Windows.Application.Current.Shutdown();
    }
}
