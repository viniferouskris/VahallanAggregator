// Create a new class file called AdminInitializer.cs
using Microsoft.AspNetCore.Identity;

public static class AdminInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                // Ensure the Admin role exists
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    logger.LogInformation("Creating Admin role");
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Check if admin user exists
                var adminEmail = "admin@yourapp.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    logger.LogInformation("Creating admin user");
                    adminUser = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin123!");

                    if (result.Succeeded)
                    {
                        logger.LogInformation("Admin user created successfully");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        logger.LogError($"Failed to create admin user: {errors}");
                        return;
                    }
                }

                // Ensure the user is in the Admin role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    logger.LogInformation("Adding user to Admin role");
                    var result = await userManager.AddToRoleAsync(adminUser, "Admin");

                    if (result.Succeeded)
                    {
                        logger.LogInformation("User added to Admin role successfully");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        logger.LogError($"Failed to add user to Admin role: {errors}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing admin user and role");
                throw;
            }
        }
    }
}