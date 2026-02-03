namespace JERP.Application.DTOs.Users;

/// <summary>
/// Request to create a new user
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Username (must be unique)
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Email address (must be unique)
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Password (will be hashed)
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Role IDs to assign to the user
    /// </summary>
    public List<Guid> RoleIds { get; set; } = new();
}
