namespace JERP.Core.Exceptions;

/// <summary>
/// Exception thrown when access to a resource is forbidden
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
