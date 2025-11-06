using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeding
{
    public static class UserSeed
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            // === Create Roles ===
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            }

            // === Create Admin ===
            if (await userManager.FindByEmailAsync("admin@foody.se") == null)
            {
                var admin = new User
                {
                    UserName = "admin@foody.se",
                    Email = "admin@foody.se",
                    FirstName = "System",
                    LastName = "Admin", // om du har LastName i modellen
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123!"); // lösenord
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }

            }

            // === Create Test Users ===
            var testUsers = new List<User>
            {
                new User { UserName = "Anna@foody.se", Email = "Anna@foody.se", FirstName = "Anna", LastName = "Andersson", EmailConfirmed = true },
            };

            foreach (var user in testUsers)
            {
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result = await userManager.CreateAsync(user, "User123!"); // samma lösen för enkelhet
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }

                }
            }
        }
    }
}
