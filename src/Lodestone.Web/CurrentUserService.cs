using Lodestone.Application.Interfaces;
using Lodestone.Shared.Extensions;
using Microsoft.AspNetCore.Http;

namespace Lodestone.Web;

/// <summary>Web implementation of <see cref="ICurrentUserService"/> over the HTTP principal.</summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    private System.Security.Claims.ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

    public string? UserId => Principal?.GetUserId();
    public string? UserName => Principal?.Identity?.Name;
    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;
    public bool IsInRole(string role) => Principal?.IsInRole(role) ?? false;
}
