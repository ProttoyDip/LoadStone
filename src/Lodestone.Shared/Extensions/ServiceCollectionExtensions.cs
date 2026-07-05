using Microsoft.Extensions.DependencyInjection;

namespace Lodestone.Shared.Extensions;

/// <summary>Cross-cutting registration helpers reused by multiple layers.</summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNamedOptions<TOptions>(this IServiceCollection services, TOptions options)
        where TOptions : class
    {
        services.AddSingleton(options);
        return services;
    }
}
