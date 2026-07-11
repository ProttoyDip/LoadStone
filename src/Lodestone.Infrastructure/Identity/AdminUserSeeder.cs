using Lodestone.Domain.Constants;
using Lodestone.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lodestone.Infrastructure.Identity;

public static class AdminUserSeeder
{
    private const string AdminEmail = "rashid.cse.20230104102@aust.edu";

    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        string adminPassword,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException("Admin seed password is missing. Set SeedData:AdminPassword or LODESTONE_ADMIN_PASSWORD.");
        }

        if (!await roleManager.RoleExistsAsync(RoleConstants.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.Admin));
        }

        var admin = await userManager.FindByEmailAsync(AdminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                FullName = "System Admin",
                EmailConfirmed = true,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
            };

            var createResult = await userManager.CreateAsync(admin, adminPassword);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Unable to seed admin user: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, RoleConstants.Admin))
        {
            await userManager.AddToRoleAsync(admin, RoleConstants.Admin);
        }
    }
}
