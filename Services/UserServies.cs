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

        public UserServies(UserManager<ApplicationUser> usermanager, ApplicationDbContext context)
        {
            _usermanager = usermanager;
            _context = context;
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
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
        /////////////// 


        public ApplicationUser Updateusert(ApplicationUser user)
        {
            _context.Update(user);
            _context.SaveChanges();
            return user;
        }
    }

}
