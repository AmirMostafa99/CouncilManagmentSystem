using CouncilsManagmentSystem.Models;

namespace CouncilsManagmentSystem.Services
{
    public interface INotificationServies
    {
        Task<object> GetobjectNotifcationById(int id);
        Task<IEnumerable<Object>> GetAllNotifcationByIdUser(string id);
        Task <Notifications> AddNotifcation (Notifications notification);
        Task<Notifications> UpdateNotifcation(Notifications not);
        Task<Notifications> GetNotifcationById(int id);
    }
}
