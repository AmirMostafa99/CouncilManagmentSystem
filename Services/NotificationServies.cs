using CouncilsManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CouncilsManagmentSystem.Services
{
    public class NotificationServies : INotificationServies
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly ICouncilsServies _councilsServies;
        

        public NotificationServies(ApplicationDbContext dbcontext, ICouncilsServies councilsServies)
        {
            _dbcontext = dbcontext;
            _councilsServies = councilsServies;
        }

        public async Task<Notifications> AddNotifcation(Notifications notification)
        {
          await _dbcontext.AddAsync(notification);
            await _dbcontext.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<object>> GetAllNotifcationByIdUser(string id)
        {
            var AllNotification = await _dbcontext.Notifications
            .Where(x => x.MemberId == id)
            .Include(n => n.CouncilMembers)
            .ThenInclude(cm => cm.Council).ThenInclude(cmm=>cmm.Hall).Select(n => new
            {
                NotificationId = n.Id,
                IsSeen = n.IsSeen,
                MemberId = n.MemberId,
                CouncilTitle = n.CouncilMembers.Council.Title,
                Date = n.CouncilMembers.Council.Date,
                Hall = n.CouncilMembers.Council.Hall.Name
            })
            .ToListAsync();

          return AllNotification;
        }

        public async Task<object> GetobjectNotifcationById(int id)
        {
            var not = await _dbcontext.Notifications.Where(x => x.Id == id)
                 .Include(n => n.CouncilMembers)
                    .ThenInclude(cm => cm.Council).ThenInclude(cmm => cmm.Hall).Select(n => new
                    {
                        NotificationId = n.Id,
                        IsSeen = n.IsSeen,
                        MemberId = n.MemberId,
                        CouncilTitle = n.CouncilMembers.Council.Title,
                        Date = n.CouncilMembers.Council.Date,
                        Hall = n.CouncilMembers.Council.Hall.Name
                    }).FirstOrDefaultAsync();

            if (not==null)
            {
                return null;
            }
            return not;
        }

        public async Task<Notifications> UpdateNotifcation(Notifications not)
        {
            _dbcontext.Update(not);
            _dbcontext.SaveChangesAsync();
            return not;
        }
        public async Task<Notifications> GetNotifcationById(int id)
        {
            var not = await _dbcontext.Notifications.FirstOrDefaultAsync(x => x.Id == id);
               

            if (not == null)
            {
                return null;
            }
            return not;
        }
    }
}
