using Microsoft.EntityFrameworkCore;

namespace Lodestone.Infrastructure.Data;

/// <summary>Applies migrations and seeds baseline reference data at startup.</summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);
        // Seed crisis resources / forum categories here as needed.
    }
}
