using CouncilsManagmentSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CouncilsManagmentSystem.DTOs;
namespace CouncilsManagmentSystem.Services
{
    public class UserServies : IUserServies
    {

        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly ApplicationDbContext _context;
        private readonly IDepartmentServies _departmentservies;

        public UserServies(UserManager<ApplicationUser> usermanager, ApplicationDbContext context , IDepartmentServies departmentservies)
        {
            _usermanager = usermanager;
            _context = context;
            _departmentservies = departmentservies;
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime(1930, 1, 1);
            DateTime endDate = new DateTime(2004, 1, 1);

            if (user.Birthday < now && user.Birthday > startDate && user.Birthday < endDate)
            {


                var result = await _usermanager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    return user;
                }
                else
                {

                    // return in error in data in users
                    throw new ApplicationException("Failed to Add user .");
                }
            }
            throw new ApplicationException("Failed to Add user .");
        }
        /////////////// 

        public ApplicationUser Deleteuser(ApplicationUser user)
        {
            _context.Remove(user);
            _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> getAllUser()
        {
            var users = await _context.Users.OrderBy(x => x.FullName).ToListAsync();
            return (users);
        }

        public async Task<IEnumerable<ApplicationUser>> getAllUserByIdCollage(int id_collage)
        {
            var users = await _usermanager.Users.Where(x => x.Department.collage_id == id_collage).ToListAsync();
            return users;
        }

        public async Task<IEnumerable<ApplicationUser>> getAllUserByIdDepartment(int id_department)
        {
            var users = await _usermanager.Users.Where(x => x.DepartmentId == id_department).ToListAsync();
            return users;
        }

        public async Task<IEnumerable<ApplicationUser>> getAllUserByname(string fullname)
        {
            var user = await _context.Users.Where(x => x.FullName.Contains(fullname)).ToListAsync();
            return user;
        }

        public async Task<ApplicationUser> getuserByEmail(string email)
        {
            var user = await _usermanager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new ApplicationException("This email is not exist .");
            }
            return user;
        }

        public async Task<ApplicationUser> getuserByFullName(string fullname)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.FullName.Contains(fullname));
            return user;
        }

        public async Task<ApplicationUser> getuserByid(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return (user);
        }

        public async Task<object> getuserObjectByid(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var collage1 = await _departmentservies.GetCollagetByIdDep(user.DepartmentId);
            var dep=await _departmentservies.GetDepartmentById(user.DepartmentId);
            //var ob = new
            //{
            //    user.Id,
            //    user.FullName,
            //    user.Email,
            //    depname= dep.name,               
            //    collage1,
            //    user.img,
            //    user.Birthday,
            //    user.academic_degree,
            //    user.administrative_degree,
            //    user.OTP,
            //    user.UserName,
            //    user.IsVerified,
            //    user.PhoneNumber

            //};
            var ob = new
            {
                user,
                collage1,
                Department = dep.name
            };
            return (ob);
        }

        public ApplicationUser Updateusert(ApplicationUser user)
        {
            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime(1970, 1, 1);
            DateTime endDate = new DateTime(2004, 1, 1);

            if (user.Birthday < now && user.Birthday > startDate && user.Birthday < endDate)
            {
                _context.Update(user);
                _context.SaveChanges();
                return user;
            }
            throw new ApplicationException("Failed to Add user .");

        }
    }

}
