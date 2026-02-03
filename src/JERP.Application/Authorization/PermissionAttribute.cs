namespace JERP.Application.Authorization;

/// <summary>
/// Attribute to specify required permission for an action
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class PermissionAttribute : Attribute
{
    /// <summary>
    /// Required permission name
    /// </summary>
    public string Permission { get; }

    public PermissionAttribute(string permission)
    {
        Permission = permission;
    }
}
