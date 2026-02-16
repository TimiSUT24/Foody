using Application.Auth.Dto.Request;
using Application.Auth.Dto.Response;
using Domain.Models;
using EllipticCurve;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using xUnitFoody.Common;

namespace xUnitFoody.Integration.Seed
{
    public static class SeedUser
    {
        public static async Task<(string Token, string UserId)> CreateAdminAndLoginAsync(HttpClient client, IServiceProvider serviceProvider)
        {
            
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }          

            var admin = new User
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                EmailConfirmed = true
            };
            var createResult = await userManager.CreateAsync(admin, "Admin123!");
            if (!createResult.Succeeded)
                throw new Exception(string.Join(", ", createResult.Errors.Select(e => e.Description)));

            var roleResult = await userManager.AddToRoleAsync(admin, "Admin");
            if (!roleResult.Succeeded)
                throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));

            // login
            var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
            {
                email = "admin@test.com",
                password = "Admin123!"
            });

            loginResponse.EnsureSuccessStatusCode();

            var json = await loginResponse.Content.ReadFromJsonAsync<LoginDtoResponse>();
            return (json!.AccessToken, admin.Id.ToString());
        }
    }
}
