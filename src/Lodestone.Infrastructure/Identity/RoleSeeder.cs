using Lodestone.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Lodestone.Infrastructure.Identity;

/// <summary>Ensures the four canonical roles exist.</summary>
public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in RoleConstants.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
