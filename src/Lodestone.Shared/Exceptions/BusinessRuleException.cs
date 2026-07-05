namespace Lodestone.Shared.Exceptions;

/// <summary>Thrown when a domain/business invariant is violated.</summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
