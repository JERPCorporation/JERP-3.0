namespace JERP.Application.DTOs.Auth;

/// <summary>
/// Request to refresh an expired access token
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Refresh token issued during login
    /// </summary>
    public required string RefreshToken { get; set; }
}
