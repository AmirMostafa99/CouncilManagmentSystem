using CouncilsManagmentSystem.Contants;
using CouncilsManagmentSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CouncilsManagmentSystem.Seeds
{
    public static class DefaultUser
    {

        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "Mohamed",
                Email = "Mohamed.20375783@compit.aun.edu.eg",
                EmailConfirmed = true,
                IsVerified = true

        };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ss1234");
                await userManager.AddToRoleAsync(defaultUser, Roles.BasicUser.ToString());
                await userManager.UpdateAsync(user);
            }

        }

        public static async Task SeedSuperAdminUserAsync(UserManager <ApplicationUser> userManager, RoleManager<IdentityRole> roleManger)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "amir",
                Email = "Amir.20375849@compit.aun.edu.eg",
                EmailConfirmed = true,
                IsVerified = true

            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ss0000");
                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.BasicUser.ToString(), Roles.SubAdmin.ToString(), Roles.SuperAdmin.ToString(), Roles.Secretary.ToString(), Roles.ChairmanOfTheBoard.ToString() });
                await userManager.UpdateAsync(user);
            }
           await roleManger.SeedClaimsForSuperUser();  
        }

        private static async Task SeedClaimsForSuperUser(this RoleManager<IdentityRole> roleManager)
        {
            var SuperAdminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            await roleManager.AddPermissionClaims(SuperAdminRole, "Councils");
        }
        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsList(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }

    }
}
