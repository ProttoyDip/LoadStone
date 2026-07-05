namespace Lodestone.Application.Interfaces;

/// <summary>Abstraction over the authenticated request principal (implemented in Web).</summary>
public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}
