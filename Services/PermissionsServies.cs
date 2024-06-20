using CouncilsManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CouncilsManagmentSystem.Services
{
    public class PermissionsServies : IPermissionsServies
    {
        private readonly ApplicationDbContext _context;

        public PermissionsServies(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Permissionss> Addpermission(Permissionss permission)
        {
            await _context.AddAsync(permission);
            await _context.SaveChangesAsync();
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
                
                default:
                    return false;
            }
        }

        public async Task<object> getObjectpermissionByid(string userid)
        {
            var per = await _context.permissionss.FirstOrDefaultAsync(x => x.userId == userid);
            var permission = new
            {
                per.AddMembers,
                per.AddTopic,
                per.AddResult,
                per.AddMembersByExcil,
                per.EditTypeCouncil,
                per.CreateTypeCouncil,
                per.EditCouncil,
                per.AddCouncil
                
            };
            return permission;

        }
    }
}
