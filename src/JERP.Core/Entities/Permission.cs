using System.ComponentModel.DataAnnotations;

namespace JERP.Core.Entities;

/// <summary>
/// Represents a specific permission that can be assigned to roles
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// Display name of the permission
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Unique code identifier for the permission
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of what the permission grants
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Category or module this permission belongs to
    /// </summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    // Navigation properties
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
