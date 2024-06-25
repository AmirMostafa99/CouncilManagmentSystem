using CouncilsManagmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace CouncilsManagmentSystem.Services
{
    public class TypeCouncilServies : ITypeCouncilServies
    {
        private readonly ApplicationDbContext _context;

        public TypeCouncilServies(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TypeCouncil> addtypecouncil(TypeCouncil typeCouncil)
        {

            await _context.AddAsync(typeCouncil);
            await _context.SaveChangesAsync();
            typeCouncil = await GetCouncilById(typeCouncil.Id);
            return typeCouncil;
        }

        public async Task<TypeCouncil> checkuser(TypeCouncil typeCouncil)
        {
            var user = await _context.typeCouncils.FirstOrDefaultAsync(x => x.Id != typeCouncil.Id && (x.SecretaryCouncilId == typeCouncil.SecretaryCouncilId || x.ChairmanCouncilId == typeCouncil.ChairmanCouncilId || x.SecretaryCouncilId == typeCouncil.ChairmanCouncilId || x.ChairmanCouncilId == typeCouncil.SecretaryCouncilId));
            var dep = await _context.typeCouncils.FirstOrDefaultAsync(x => x.DepartmentId == typeCouncil.DepartmentId && x.Id != typeCouncil.Id);
            if (user != null)
            {
                throw new ApplicationException("Users is exist !");
            }
            if (dep != null)
            {
                throw new ApplicationException("Department is exist !");
            }
            return null;
        }
        
        public async Task<TypeCouncil> GetCouncilById(int id)
        {
            var typeCouncil = await _context.typeCouncils
                .Where(x => x.Id == id)
                .Select(z => new TypeCouncil
                {
                    Id = z.Id,
                    Name = z.Name,
                    ChairmanCouncilId = z.ChairmanCouncilId,
                    SecretaryCouncilId = z.SecretaryCouncilId,
                    DepartmentId = z.DepartmentId
                })
                .FirstOrDefaultAsync();

            if (typeCouncil == null)
            {
                throw new ApplicationException("Council not found for the given department.");
            }

            return typeCouncil;
        }

        public async Task<TypeCouncil> GetUserOfTypeCouncil(string iduser)
        {
            var user = await _context.typeCouncils.FirstOrDefaultAsync(x =>  x.SecretaryCouncilId == iduser || x.ChairmanCouncilId == iduser );
            if(user == null)
            {
                return null;
            }
            return user;
        
        }

        public async Task<IEnumerable<TypeCouncil>> listtypecouncil()
        {
            var types = await _context.typeCouncils.OrderBy(x => x.Name).ToListAsync();
            return types;
        }

        public TypeCouncil updateTypeCouncilAsync(TypeCouncil typeCouncil)
        {
            _context.Update(typeCouncil);
            _context.SaveChanges();

            return typeCouncil;
        }


    }

}
