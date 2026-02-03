using System.ComponentModel.DataAnnotations;

namespace JERP.Core.Entities;

/// <summary>
/// Represents a security role that can be assigned to users
/// </summary>
public class Role : BaseEntity
{
    /// <summary>
    /// Name of the role
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the role's purpose and responsibilities
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
