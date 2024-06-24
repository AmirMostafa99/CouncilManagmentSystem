using CouncilsManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CouncilsManagmentSystem.Services
{
    public class PermissionsServies : IPermissionsServies
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserServies _userServies;

        public PermissionsServies(ApplicationDbContext context, IUserServies userServies)
        {
            _context = context;
            _userServies = userServies;
        }
        public async Task<object> Addpermission(Permissionss per)
        {
            await _context.AddAsync(per);
            await _context.SaveChangesAsync();
            var permission = new
            {
                per.AddMembers,
                per.AddTopic,
                per.AddResult,
                per.AddMembersByExcil,
                per.EditTypeCouncil,
                per.CreateTypeCouncil,
                per.EditCouncil,
                per.AddCouncil,
                per.Arrange,
                per.AddCollage,
                per.AddDepartment,
                per.AddHall,
                per.Updatepermission,
                per.DeactiveUser,
                per.UpdateUser

            };
            return permission;
           
        }

        public async Task<Permissionss> getpermissionByid(string userid)
        {
            var per=await _context.permissionss.FirstOrDefaultAsync(x=>x.userId==userid);
            return per;
        }
        public async Task<bool> CheckPermissionAsync(string userId, string permissionName)
        {
            var permissions = await _context.permissionss
                .FirstOrDefaultAsync(p => p.userId == userId);

            if (permissions == null)
            {
                return false;
            }

            switch (permissionName)
            {
                case "AddMembers":
                    return permissions.AddMembers;
                case "AddTopic":
                    return permissions.AddTopic;
                case "AddResult":
                    return permissions.AddResult;
                case "AddMembersByExcil":
                    return permissions.AddMembersByExcil;
                case "EditTypeCouncil":
                    return permissions.EditTypeCouncil;
                case "CreateTypeCouncil":
                    return permissions.CreateTypeCouncil;
                case "EditCouncil":
                    return permissions.EditCouncil;
                case "AddCouncil":
                    return permissions.AddCouncil;
                case "UpdateUser":
                    return permissions.UpdateUser;
                case "DeactiveUser":
                    return permissions.DeactiveUser;
                case "Updatepermission":
                    return permissions.Updatepermission;
                case "AddCollage":
                    return permissions.AddCollage;
                case "AddDepartment":
                    return permissions.AddDepartment;
                case "AddHall":
                    return permissions.AddHall;

                default:
                    return false;
            }
        }

        public async Task<object> getObjectpermissionByid(string userid)
        {
            var per = await _context.permissionss.FirstOrDefaultAsync(x => x.userId == userid);
            if (per != null)
            {


                var user = await _userServies.getuserByid(userid);
                var permission = new
                {
                    user.FullName,
                    user.Email,
                    per.AddMembers,
                    per.AddTopic,
                    per.AddResult,
                    per.AddMembersByExcil,
                    per.EditTypeCouncil,
                    per.CreateTypeCouncil,
                    per.EditCouncil,
                    per.AddCouncil,
                    per.Arrange,
                    per.AddCollage,
                    per.AddDepartment,
                    per.AddHall,
                    per.Updatepermission,
                    per.DeactiveUser,
                    per.UpdateUser

                };
                return permission;
            }
            else
            {
                return null;
            }

        }

        public async Task<object> UpdatePermission(Permissionss per)
        {
            _context.Update(per);
            _context.SaveChanges();
            var permission = new
            {
                per.AddMembers,
                per.AddTopic,
                per.AddResult,
                per.AddMembersByExcil,
                per.EditTypeCouncil,
                per.CreateTypeCouncil,
                per.EditCouncil,
                per.AddCouncil,
                per.Arrange,
                per.AddCollage,
                per.AddDepartment,
                per.AddHall,
                per.Updatepermission,
                per.DeactiveUser,
                per.UpdateUser
                

            };
            return permission;
           
        }
    }
}
