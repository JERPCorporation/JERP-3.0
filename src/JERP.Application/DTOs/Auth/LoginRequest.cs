namespace JERP.Application.DTOs.Auth;

/// <summary>
/// Login request containing user credentials
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Username for authentication
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Password for authentication
    /// </summary>
    public required string Password { get; set; }
}
