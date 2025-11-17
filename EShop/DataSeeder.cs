using EShop.Context;
using EShop.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EShop
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            try
            {
                await context.Database.MigrateAsync();

                Log.Information("Starting data seeding...");

                if (!await context.Roles.AnyAsync())
                {
                    var roles = new List<Role>
                    {
                        new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Administrator role"},
                        new Role { Id = Guid.NewGuid(), Name = "User", Description = "Administrator role"}
                    };

                    await context.Roles.AddRangeAsync(roles);
                    await context.SaveChangesAsync();

                    Log.Information("Default roles seeded.");
                }

                if (!await context.Users.AnyAsync(u => u.Email == "yoyo2510@gmail.com"))
                {
                    var adminUser = new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = "yoyo",
                        Email = "yoyo2510@gmail.com",
                        PassWord = "yoyo2008",
                        PhoneNumber = "08012345678",
                        Address = "System Default",
                        Gender = Gender.Male,
                    };

                    await context.Users.AddAsync(adminUser);
                    await context.SaveChangesAsync();

                    Log.Information("Default Admin user created.");

                    var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                    if (adminRole != null)
                    {
                        var userRole = new UserRole
                        {
                            Id = Guid.NewGuid(),
                            UserId = adminUser.Id,
                            RoleId = adminRole.Id
                        };

                        await context.UserRoles.AddAsync(userRole);
                        await context.SaveChangesAsync();
                        Log.Information("Admin role assigned to default admin user.");
                    }
                }

                Log.Information("Data seeding completed successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during data seeding.");
                throw;
            }
        }
    }
}
