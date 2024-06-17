using CouncilsManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CouncilsManagmentSystem.Services
{
    public class CouncilMembersServies : ICouncilMembersServies
    {

        private readonly ApplicationDbContext _context;
        private readonly ICouncilsServies _councilsServies;
        private readonly IUserServies _userServies;

        public CouncilMembersServies(ApplicationDbContext context, ICouncilsServies councilsServies = null, IUserServies userServies = null)
        {
            _context = context;
            _councilsServies = councilsServies;
            _userServies = userServies;
        }


        public async Task<string> Addmember(CouncilMembers member)
        {
            await _context.AddAsync(member);
            await _context.SaveChangesAsync();
            return "success";
        }

        public Task<string> delete(CouncilMembers council)
        {
            _context.Remove(council);
            _context.SaveChanges();
            return null;
        }

        public async Task<IEnumerable<object>> GetAllCouncilsbyEmailmember(string email)
        {

            var user = await _userServies.getuserByEmail(email);
            if (user == null)
            {
                return null;
            }
            var councils = await _context.CouncilMembers.Where(x => x.MemberId == user.Id).Include(a => a.Council).Select(z => new {
                id = z.Council.Id,
                Title = z.Council.Title
            }).ToListAsync();
            return councils;
        }



        public async Task<IEnumerable<object>> GetAllCouncilsbyidmember(string id)
        {
            var user = await _userServies.getuserByid(id);
            if (user == null)
            {
                return null;
            }
            var councils = await _context.CouncilMembers.Where(x => x.MemberId == id).Include(a => a.Council).Select(z => new {
                id = z.Council.Id
                ,
                Title = z.Council.Title
            }).ToListAsync();
            return councils;
        }



        public async Task<IEnumerable<object>> GetAllMembersbyidCouncil(int id)
        {
            var council = await _councilsServies.GetCouncilById(id);
            if (council == null)
            {
                return null;
            }
            var users = await _context.CouncilMembers
                .Where(x => x.CouncilId == id).Include(x => x.ApplicationUser).Select(z => new { fullname = z.ApplicationUser.FullName, Email = z.ApplicationUser.Email })
                .ToListAsync();
            return users;
        }

        public async Task<object> GetCouncilbyEmailmember(string email, int council)
        {

            var user = await _userServies.getuserByEmail(email);
            if (user == null)
            {
                return null;
            }
            var councils = await _context.CouncilMembers.FirstOrDefaultAsync(x => x.CouncilId == council && x.MemberId == user.Id);
            if (councils != null)
            {
                return "In this council";
            }
            return "Not in this council";

        }

        public async Task<CouncilMembers> GetcouncilmemberlById(int councilId, string userId)
        {
            var council = await _context.CouncilMembers.FirstOrDefaultAsync(x => x.CouncilId == councilId && x.MemberId == userId);
            return council;
        }
    }
}