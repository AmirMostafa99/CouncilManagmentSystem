using CouncilsManagmentSystem.Models;

namespace CouncilsManagmentSystem.Services
{
    public interface IPermissionsServies
    {
        Task<object> Addpermission(Permissionss permission);
        Task<Permissionss> getpermissionByid(string userid);
        Task<object> getObjectpermissionByid(string userid);
        Task<bool> CheckPermissionAsync(string userId, string permissionName);
        Task<object> UpdatePermission(Permissionss permission);

    }
}
