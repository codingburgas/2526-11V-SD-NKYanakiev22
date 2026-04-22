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
        await EnsureBusinessesSchemaAsync(context);

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

            await SeedExampleBusinessesAsync(context, adminUser);
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
            await SeedExampleBusinessesAsync(context, adminUser);
        }
    }

    private static async Task EnsureBusinessesSchemaAsync(ApplicationDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync(
            """
            CREATE TABLE IF NOT EXISTS Businesses (
                Id INTEGER NOT NULL CONSTRAINT PK_Businesses PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                OwnerName TEXT NOT NULL,
                Email TEXT NOT NULL,
                PhoneNumber TEXT NOT NULL,
                Industry TEXT NOT NULL,
                Address TEXT NOT NULL,
                Description TEXT NOT NULL,
                CreatedByUserId TEXT NOT NULL,
                CreatedOnUtc TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                ModifiedOnUtc TEXT NULL,
                IsSeededExample INTEGER NOT NULL DEFAULT 0,
                CONSTRAINT FK_Businesses_AspNetUsers_CreatedByUserId FOREIGN KEY (CreatedByUserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
            );
            """);

        await EnsureColumnAsync(context, "Businesses", "ModifiedOnUtc", "TEXT NULL");
        await EnsureColumnAsync(context, "Businesses", "IsSeededExample", "INTEGER NOT NULL DEFAULT 0");
        await context.Database.ExecuteSqlRawAsync(
            "CREATE INDEX IF NOT EXISTS IX_Businesses_CreatedByUserId ON Businesses (CreatedByUserId);");

        await context.Database.ExecuteSqlRawAsync(
            """
            CREATE TABLE IF NOT EXISTS BusinessInvestments (
                Id INTEGER NOT NULL CONSTRAINT PK_BusinessInvestments PRIMARY KEY AUTOINCREMENT,
                BusinessId INTEGER NOT NULL,
                InvestorUserId TEXT NOT NULL,
                InvestorName TEXT NOT NULL,
                Amount TEXT NOT NULL,
                Note TEXT NOT NULL,
                CreatedOnUtc TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                ModifiedOnUtc TEXT NULL,
                CONSTRAINT FK_BusinessInvestments_Businesses_BusinessId FOREIGN KEY (BusinessId) REFERENCES Businesses (Id) ON DELETE CASCADE
            );
            """);

        await EnsureColumnAsync(context, "BusinessInvestments", "ModifiedOnUtc", "TEXT NULL");
        await context.Database.ExecuteSqlRawAsync(
            "CREATE INDEX IF NOT EXISTS IX_BusinessInvestments_BusinessId ON BusinessInvestments (BusinessId);");
    }

    private static async Task EnsureColumnAsync(
        ApplicationDbContext context,
        string tableName,
        string columnName,
        string columnDefinition)
    {
        await context.Database.OpenConnectionAsync();

        try
        {
            var connection = context.Database.GetDbConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = $"""
                                   SELECT COUNT(*)
                                   FROM pragma_table_info('{tableName}')
                                   WHERE name = '{columnName}'
                                   """;

            var result = await command.ExecuteScalarAsync();
            var hasColumn = Convert.ToInt32(result) > 0;

            if (!hasColumn)
            {
                var alterStatement = tableName switch
                {
                    "Businesses" when columnName == "ModifiedOnUtc" =>
                        "ALTER TABLE Businesses ADD COLUMN ModifiedOnUtc TEXT NULL;",
                    "Businesses" when columnName == "IsSeededExample" =>
                        "ALTER TABLE Businesses ADD COLUMN IsSeededExample INTEGER NOT NULL DEFAULT 0;",
                    "BusinessInvestments" when columnName == "ModifiedOnUtc" =>
                        "ALTER TABLE BusinessInvestments ADD COLUMN ModifiedOnUtc TEXT NULL;",
                    _ => throw new InvalidOperationException("Unsupported schema change.")
                };

                await context.Database.ExecuteSqlRawAsync(
                    alterStatement);
            }
        }
        finally
        {
            await context.Database.CloseConnectionAsync();
        }
    }

    private static async Task SeedExampleBusinessesAsync(ApplicationDbContext context, ApplicationUser adminUser)
    {
        var examples = new[]
        {
            new
            {
                Name = "Vitosha Trade",
                Industry = "Retail",
                Email = "contact@vitoshatrade.bg",
                PhoneNumber = "+359888000111",
                Address = "Sofia, Bulgaria",
                Description = "Retail business focused on local distribution, storefront sales, and supplier coordination.",
                OwnerName = adminUser.FullName
            },
            new
            {
                Name = "Danube Logistics",
                Industry = "Logistics",
                Email = "office@danubelogistics.bg",
                PhoneNumber = "+359887000222",
                Address = "Ruse, Bulgaria",
                Description = "Logistics company handling regional delivery planning, fleet operations, and commercial fulfillment.",
                OwnerName = adminUser.FullName
            }
        };

        foreach (var example in examples)
        {
            var exists = await context.Businesses.AnyAsync(business => business.Name == example.Name);
            if (exists)
            {
                continue;
            }

            context.Businesses.Add(new()
            {
                Name = example.Name,
                Industry = example.Industry,
                Email = example.Email,
                PhoneNumber = example.PhoneNumber,
                Address = example.Address,
                Description = example.Description,
                OwnerName = example.OwnerName,
                CreatedByUserId = adminUser.Id,
                IsSeededExample = true,
                CreatedOnUtc = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
    }
}
