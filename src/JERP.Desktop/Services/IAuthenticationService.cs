using JERP.Application.DTOs;

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
