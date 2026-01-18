using AuthAPI.Data.Entities;
using AuthAPI.Extensions;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data
{
    public class Seed
    {
        public static async Task SeedData(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            await EnsureRoleAsync(roleManager, "Member");
            await EnsureRoleAsync(roleManager, "Saler");
            await EnsureRoleAsync(roleManager, "Admin");

            await EnsureUserAsync(
                userManager,
                "Admin", 
                "admin@test.com", 
                "Admin123!", 
                "Admin");
        }

        private static async Task EnsureRoleAsync(RoleManager<AppRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new AppRole(roleName));
            }
        }

        private static async Task EnsureUserAsync(
            UserManager<AppUser> userManager, 
            string username, 
            string email, 
            string password, 
            string role)
        {
            if (await userManager.UserExistsAsync(username)) return;
            
            var user = new AppUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            
        }
    }
}
