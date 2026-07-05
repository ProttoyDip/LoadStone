namespace Lodestone.Shared.Exceptions;

/// <summary>Thrown when an authenticated user lacks rights for an action (app-level, not HTTP).</summary>
public class UnauthorizedAccessAppException : Exception
{
    public UnauthorizedAccessAppException(string message) : base(message) { }
}
