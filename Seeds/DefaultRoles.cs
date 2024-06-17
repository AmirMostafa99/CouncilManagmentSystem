using CouncilsManagmentSystem.Contants;
using Microsoft.AspNetCore.Identity;

namespace CouncilsManagmentSystem.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManger)
        {
            if (!roleManger.Roles.Any())
            {
        
                await roleManger.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                await roleManger.CreateAsync(new IdentityRole(Roles.SubAdmin.ToString()));
                await roleManger.CreateAsync(new IdentityRole(Roles.BasicUser.ToString()));
                await roleManger.CreateAsync(new IdentityRole(Roles.Secretary.ToString()));
                await roleManger.CreateAsync(new IdentityRole(Roles.ChairmanOfTheBoard.ToString()));
            }
        }
    }
}
