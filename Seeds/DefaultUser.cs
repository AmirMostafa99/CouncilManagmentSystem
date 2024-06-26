using CouncilsManagmentSystem.Contants;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CouncilsManagmentSystem.Seeds
{
    public static class DefaultUser
    {
        private static readonly IPermissionsServies permissionsServies;
        private static readonly ICollageServies collageServies;
        private static readonly IDepartmentServies departmentServies;
        private static readonly ApplicationDbContext dbContext;


        //public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager)
        //{

        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            using (var context = dbContext ?? new ApplicationDbContext())
            {
                var collage = new Collage { Name = "Fci" };
                var coll = await dbContext.collages.FirstOrDefaultAsync(x => x.Name == "Fci");
                if (coll == null)
                {
                    await context.AddAsync(collage);
                    await context.SaveChangesAsync();




                    var department = new Department { collage_id = collage.Id, name = "Is" };
                    var dep = await dbContext.departments.FirstOrDefaultAsync(x => x.name == "Is");
                    if (dep == null)
                    {
                        await context.AddAsync(department);
                        await context.SaveChangesAsync();

                    }



                    var defaultUser = new ApplicationUser
                    {
                        FullName = "Mohamed",
                        UserName = "Mohamed",
                        Email = "Mohamed.20375783@compit.aun.edu.eg",
                        EmailConfirmed = true,
                        IsVerified = true,
                        DepartmentId = department.id

                    };


                    var user = await userManager.FindByEmailAsync(defaultUser.Email);


                    if (user == null)
                    {
                        await userManager.CreateAsync(defaultUser, "P@ss1234");
                        await dbContext.SaveChangesAsync();
                        //await userManager.AddToRoleAsync(defaultUser, Roles.BasicUser.ToString());
                       // await userManager.UpdateAsync(user);
                    }

                    var permissions = new Permissionss
                    {
                        userId = defaultUser.Id,
                        AddCouncil = true,
                        EditCouncil = true,
                        CreateTypeCouncil = true,
                        EditTypeCouncil = true,
                        AddMembersByExcil = true,
                        AddMembers = true,
                        AddTopic = true,
                        Arrange = true,
                        AddResult = true,
                        AddDepartment = true,
                        AddCollage = true,
                        Updatepermission = true,
                        DeactiveUser = true,
                        UpdateUser = true,
                        AddHall = true

                    };
                    await dbContext.AddAsync(permissions);
                    await dbContext.SaveChangesAsync();

                }
                

            }

        }

        public static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger, ApplicationDbContext dbContext)
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

