namespace Lodestone.Shared.Result;

/// <summary>A machine-readable error carried by a failed <see cref="Result"/>.</summary>
public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
