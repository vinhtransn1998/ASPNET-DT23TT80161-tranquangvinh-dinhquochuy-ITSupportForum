using Microsoft.AspNetCore.Identity;

namespace ITSupportForum.Data
{
    public static class SeedData
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            var adminEmail = "admin@gmail.com";
            var adminPassword = "Admin123!";

            var adminUser =
                await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result =
                    await userManager.CreateAsync(
                        user,
                        adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        user,
                        "Admin");
                }
            }
        }
    }
}