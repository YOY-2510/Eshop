using EShop.Context;
using EShop.Data;
using Microsoft.EntityFrameworkCore;

namespace EShop
{
    public class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            var adminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Admin");

            if (adminRole == null)
            {
                adminRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Description = "Default admin role"
                };

                await context.Roles.AddAsync(adminRole);
                await context.SaveChangesAsync();
            }

            var adminUser = await context.Users
                .FirstOrDefaultAsync(u => u.Email == "Justdeyplay@gmail.com");

            if (adminUser == null)
            {
                adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "Admin",
                    Email = "AseebPaulo@gmail.com",
                    PassWord = "050310"
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }

            var userRole = await context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id);

            if (userRole == null)
            {
                userRole = new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id
                };

                await context.UserRoles.AddAsync(userRole);
                await context.SaveChangesAsync();
            }
        }
    }
}
