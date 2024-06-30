using CouncilsManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CouncilsManagmentSystem.Services
{
    public class CouncilsServies : ICouncilsServies
    {
        private readonly ApplicationDbContext _context;
        
        private readonly ITypeCouncilServies _typeCouncilServies;

        public CouncilsServies(ApplicationDbContext context,  ITypeCouncilServies typeCouncilServies)
        {
            _context = context;
          
            _typeCouncilServies = typeCouncilServies;
        }
            
        public async Task<object> AddCouncil(Councils council)
        {
            var type = await _typeCouncilServies.GetCouncilById(council.TypeCouncilId);
            if (type == null)
            {
                throw new ApplicationException("Failed to Add Council please review data .");
            }
            DateTime now = DateTime.Now;
            if (council.Date > now)
            {
                await _context.AddAsync(council);
                await _context.SaveChangesAsync();
                var re = new Councils { Id = council.Id, Title = council.Title, Date = council.Date , HallId=council.HallId , TypeCouncilId=council.TypeCouncilId };
                
                return re ;
            }
            throw new ApplicationException("Please check Date");
        }

        public async Task<IEnumerable<Councils>> GetallCouncils()
        {
            var councils = await _context.Councils.OrderBy(x => x.Title).ToListAsync();
            return councils;
        }

        public async Task<IEnumerable<Object>> GetAllCouncilsByIdType(int typeId)
        {
            var type = await _typeCouncilServies.GetCouncilById(typeId);
            if (type == null)
            {
                throw new ApplicationException("Failed to Add Council please review data .");
            }
            var startDate = DateTime.Now.Date;
            var councils=await _context.Councils.Where(x => x.TypeCouncilId == typeId && x.Date.Date>startDate).Include(z => z.Hall).Select(z => new
            {
                id=z.Id
                , title=z.Title
                ,Date=z.Date
                ,Hall=z.Hall.Name

            }).ToListAsync();
            return councils;
        }

        public async Task<IEnumerable<object>> GetAllLastCouncilsByIdType(int typeId)
        {
            var type = await _typeCouncilServies.GetCouncilById(typeId);
            if (type == null)
            {
                throw new ApplicationException("Failed to Add Council please review data .");
            }
            var startDate = DateTime.Now.Date;
            var councils = await _context.Councils.Where(x => x.TypeCouncilId == typeId && x.Date.Date < startDate).Include(z => z.Hall).Select(z => new
            {
                id = z.Id
                ,
                title = z.Title
                ,
                Date = z.Date
                ,
                Hall = z.Hall.Name

            }).ToListAsync();
            return councils;
        }

        public async Task<Councils> GetCouncilByDate(DateTime date)
        {
            var council = await _context.Councils.FirstOrDefaultAsync(x => x.Date.Date == date.Date);
            if(council == null)
            {
                throw new ApplicationException("Dont have any council in this data !");
            }
            return council;
        }

        public async Task<Councils> GetCouncilById(int councilId)
        {
            var council = await _context.Councils.FirstOrDefaultAsync(x => x.Id==councilId);
            if (council == null)
            {
                throw new ApplicationException("Dont have this ID !");
            }
            return council;
        }

        public async Task<IEnumerable<Councils>> GetCouncilSbyDate(DateTime date)
        {
            if (date.Date > DateTime.Now.Date)
            {
                var startDate = DateTime.Now.Date;
                var endDate = date.Date;

                var councils = await _context.Councils
                    .Where(x => x.Date.Date >= startDate && x.Date.Date <= endDate)
                    .ToListAsync();
                return councils;

            }
            else
            {
                var endDate = DateTime.Now.Date;
                var startDate = date.Date;

                var councils = await _context.Councils
                   .Where(x => x.Date.Date >= startDate && x.Date.Date <= endDate)
                   .ToListAsync();
                return councils;
            }

        }

        public async Task<IEnumerable<Councils>> GetCouncilSbyIDhalls(int Idhall)
        {
            var council = await _context.Councils.Where(x => x.HallId==Idhall).ToListAsync();
            if (council == null)
            {
                throw new ApplicationException("Dont have any council in ID halls !");
            }
            return council;
        }

        public async Task<IEnumerable<Councils>> GetCouncilsByTitle(string name)
        {
            var council = await _context.Councils.Where(x => x.Title.Contains(name)).ToListAsync();
            if (council == null)
            {
                throw new ApplicationException("Dont have any council has this name !");
            }
            return council;
        }

        public async Task<string> UpdateCouncil(Councils council)
        {
            var type = await _typeCouncilServies.GetCouncilById(council.TypeCouncilId);
            if (type == null)
            {
                throw new ApplicationException("Failed to Add Council please review data .");
            }
            DateTime now = DateTime.Now;
            if (council.Date > now)
            {
                 _context.Update(council);
                await _context.SaveChangesAsync();
                return "success";
            }
           
            return "Please check Date";
        }
    }
}
