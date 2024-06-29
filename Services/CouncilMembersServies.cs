using CouncilsManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace CouncilsManagmentSystem.Services
{
    public class CouncilMembersServies : ICouncilMembersServies
    {

        private readonly ApplicationDbContext _context;
        private readonly ICouncilsServies _councilsServies;
        private readonly IUserServies _userServies;
        private readonly ITypeCouncilServies _typeCouncilServies;

        public CouncilMembersServies(ApplicationDbContext context, ICouncilsServies councilsServies = null, IUserServies userServies = null, ITypeCouncilServies typeCouncilServies = null)
        {
            _context = context;
            _councilsServies = councilsServies;
            _userServies = userServies;
            _typeCouncilServies = typeCouncilServies;
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
                Date = z.Council.Date,
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
                Date = z.Council.Date,
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
        //next councils
        public async Task<IEnumerable<object>> GetAllNextCouncilsbyidmember(string idmember)
        {
            var user = await _userServies.getuserByid(idmember);
            if (user == null)
            {
                return null;
            }

            DateTime now = DateTime.Now;

            var councils = await _context.CouncilMembers
                .Where(x => x.MemberId == idmember)
                .Include(a => a.Council).ThenInclude(a=>a.Hall)
                .Select(z => new
                {
                    id = z.Council.Id,
                    Date = z.Council.Date,
                    CurrentDateTime = now,
                    Title = z.Council.Title,
                    Hall=z.Council.Hall.Name
                })
                .Where(z => z.Date > z.CurrentDateTime)
                .ToListAsync();

            return councils.Select(z => new
            {
                z.id,
                z.Title,
                z.Date,
                z.Hall
            });
        }

        public async Task<IEnumerable<ApplicationUser>> getAllUserInDep(int typecouncilId)
        {
            var type = await _typeCouncilServies.GetCouncilById(typecouncilId);
            var users = await _userServies.getAllUserByIdDepartment(type.DepartmentId);
            if (users != null)
            {
                return users;
            }
            return null;

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

        public CouncilMembers updatecouncilmember(CouncilMembers member)
        {
            _context.Update(member);
            _context.SaveChanges();

            return member;
        }

        public async Task<IEnumerable<CouncilMembers>> GetAllIDMembersbyidCouncil(int id)
        {
            var council = await _councilsServies.GetCouncilById(id);
            if (council == null)
            {
                return null;
            }
            var users = await _context.CouncilMembers
                .Where(x => x.CouncilId == id)
                .ToListAsync();
            return users;
        }

        public async Task<IEnumerable<object>> GetAllCouncilMemberIsNotAtt( int idcouncil)
        {
             var mem = await _context.CouncilMembers
            .Where( x => x.IsAttending == false  && x.CouncilId==idcouncil)
            .Include(a => a.ApplicationUser )
            .Select(a => new
            {
                UserName = a.ApplicationUser.FullName,
                Email = a.ApplicationUser.Email,
                Reason = a.ReasonNonAttendance
                
            }).ToListAsync();

            return mem;

        }

        public async Task<IEnumerable<object>> GetAllLastCouncilsbyidmember(string idmember)
        {
            var user = await _userServies.getuserByid(idmember);
            if (user == null)
            {
                return null;
            }

            DateTime now = DateTime.Now;

            var councils = await _context.CouncilMembers
                .Where(x => x.MemberId == idmember)
                .Include(a => a.Council).ThenInclude(a => a.Hall)
                .Select(z => new
                {
                    id = z.Council.Id,
                    Date = z.Council.Date,
                    CurrentDateTime = now,
                    Title = z.Council.Title,
                    Hall = z.Council.Hall.Name
                })
                .Where(z => z.Date < z.CurrentDateTime)
                .ToListAsync();

            return councils.Select(z => new
            {
                z.id,
                z.Title,
                z.Date,
                z.Hall
            });
        }
    }
}
