using CouncilsManagmentSystem.Models;

namespace CouncilsManagmentSystem.Services
{
    public interface IUserServies
    {
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> getAllUser();
        Task<ApplicationUser> getuserByid(string id);
        Task<ApplicationUser> getuserByEmail(string email);
        Task<ApplicationUser> getuserByFullName(string fullname);
        Task<IEnumerable<ApplicationUser>> getAllUserByname(string fullname);
        Task<IEnumerable<ApplicationUser>> getAllUserByIdDepartment(int id_department);
        Task<IEnumerable<ApplicationUser>> getAllUserByIdCollage(int id_collage);
        ApplicationUser Updateusert(ApplicationUser user);
        ApplicationUser Deleteuser(ApplicationUser user);
        Task<object> getuserObjectByid(string id);
    }
}
