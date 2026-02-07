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

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JERP.Desktop.Services;

namespace JERP.Desktop.ViewModels;

public partial class AIAssistantViewModel : ViewModelBase
{
    private readonly IApiClient _apiClient;

    [ObservableProperty]
    private string _userQuestion = string.Empty;

    [ObservableProperty]
    private ObservableCollection<ChatMessage> _messages = new();

    [ObservableProperty]
    private bool _isSending;

    public AIAssistantViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        
        // Welcome message
        Messages.Add(new ChatMessage
        {
            Role = "assistant",
            Content = "Hello! I'm the JERP AI Assistant powered by Claude. I can help you with accounting questions, tax compliance, journal entries, financial reports, and more. What would you like to know?",
            Timestamp = DateTime.Now
        });
    }

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(UserQuestion) || IsSending)
            return;

        var question = UserQuestion.Trim();
        UserQuestion = string.Empty;

        // Add user message to chat
        Messages.Add(new ChatMessage
        {
            Role = "user",
            Content = question,
            Timestamp = DateTime.Now
        });

        IsSending = true;

        try
        {
            var response = await _apiClient.PostAsync<AiAssistantResponse>(
                "api/aiassistant/ask",
                new { Question = question });

            Messages.Add(new ChatMessage
            {
                Role = "assistant",
                Content = response?.Answer ?? "I couldn't process that request. Please try again.",
                Timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            Messages.Add(new ChatMessage
            {
                Role = "assistant",
                Content = $"Sorry, I encountered an error: {ex.Message}. Please make sure the API is running and the Claude API key is configured.",
                Timestamp = DateTime.Now
            });
        }
        finally
        {
            IsSending = false;
        }
    }

    [RelayCommand]
    private void ClearChat()
    {
        Messages.Clear();
        Messages.Add(new ChatMessage
        {
            Role = "assistant",
            Content = "Chat cleared. How can I help you?",
            Timestamp = DateTime.Now
        });
    }
}

public class ChatMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool IsUser => Role == "user";
    public bool IsAssistant => Role == "assistant";
}

public class AiAssistantResponse
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
