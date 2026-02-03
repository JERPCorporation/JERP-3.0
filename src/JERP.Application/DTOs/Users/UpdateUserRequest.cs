namespace JERP.Application.DTOs.Users;

/// <summary>
/// Request to update an existing user
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Email address
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Whether the user account is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Role IDs to assign to the user
    /// </summary>
    public List<Guid> RoleIds { get; set; } = new();
}
