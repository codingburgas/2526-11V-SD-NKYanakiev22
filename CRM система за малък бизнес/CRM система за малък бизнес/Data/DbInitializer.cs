using CrmSmallBusiness.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CrmSmallBusiness.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.EnsureCreatedAsync();

        foreach (var role in AppRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        const string legacyAdminEmail = "admin@crm-demo.local";
        const string adminEmail = "admin@gmail.com";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = await userManager.FindByEmailAsync(legacyAdminEmail);
        }

        if (adminUser is not null)
        {
            if (!string.Equals(adminUser.Email, adminEmail, StringComparison.OrdinalIgnoreCase))
            {
                adminUser.Email = adminEmail;
                adminUser.UserName = adminEmail;
                adminUser.NormalizedEmail = adminEmail.ToUpperInvariant();
                adminUser.NormalizedUserName = adminEmail.ToUpperInvariant();
                await userManager.UpdateAsync(adminUser);
            }

            if (!await userManager.IsInRoleAsync(adminUser, AppRoles.Admin))
            {
                var existingRoles = await userManager.GetRolesAsync(adminUser);
                if (existingRoles.Count > 0)
                {
                    await userManager.RemoveFromRolesAsync(adminUser, existingRoles);
                }

                await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
            }

            return;
        }

        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "CRM Administrator",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
        }
    }
}
