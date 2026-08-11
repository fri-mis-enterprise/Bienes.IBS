using IBS.Models;
using IBS.Utility.Constants;
using IBS.Utility.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IBS.DataAccess.Data
{
    public static class ApplicationDbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = ["User", "Admin"];

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    IdentityResult roleResult = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!roleResult.Succeeded)
                    {
                        string errors = string.Join(", ", roleResult.Errors.Select(error => error.Description));
                        throw new InvalidOperationException($"Failed to seed role '{role}': {errors}");
                    }
                }
            }

            const string username = "azh";
            const string name = "AZH ADOLFO";
            const string tempPassword = "Testing.123456";

        ApplicationUser? user = await userManager.FindByNameAsync(username);

        if (user == null)
        {
            user = new ApplicationUser
                {
                    UserName = username,
                    Name = name,
                    Department = SD.Department_MIS,
                    IsActive = true,
                    CreatedDate = DateTimeHelper.GetCurrentPhilippineTime()
                };

                IdentityResult createUserResult = await userManager.CreateAsync(user, tempPassword);

            if (!createUserResult.Succeeded)
            {
                string errors = string.Join(", ", createUserResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to seed user '{username}': {errors}");
            }
            IdentityResult addAdminRoleResult = await userManager.AddToRoleAsync(user, "Admin");

            if (!addAdminRoleResult.Succeeded)
            {
                string errors = string.Join(", ", addAdminRoleResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to assign Admin role to '{username}': {errors}");
            }

            IdentityResult addUserRoleResult = await userManager.AddToRoleAsync(user, "User");

            if (!addUserRoleResult.Succeeded)
            {
                string errors = string.Join(", ", addUserRoleResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to assign User role to '{username}': {errors}");
            }
        }
    }
}
}
