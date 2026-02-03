using JERP.Application.DTOs.Users;

namespace JERP.Application.DTOs.Auth;

/// <summary>
/// Authentication token response
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// Refresh token for obtaining new access tokens
    /// </summary>
    public required string RefreshToken { get; set; }

    /// <summary>
    /// Token expiration timestamp
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Authenticated user information
    /// </summary>
    public required UserDto User { get; set; }
}
