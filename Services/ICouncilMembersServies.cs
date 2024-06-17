using CouncilsManagmentSystem.Models;

namespace CouncilsManagmentSystem.Services
{
    public interface ICouncilMembersServies
    {
        Task<IEnumerable<object>> GetAllMembersbyidCouncil(int id);
        Task<CouncilMembers> GetcouncilmemberlById(int councilId, string userId);
        Task<string> Addmember(CouncilMembers member);
        Task<IEnumerable<object>> GetAllCouncilsbyidmember(string id);
        Task<IEnumerable<object>> GetAllCouncilsbyEmailmember(string email);
        Task<string> delete(CouncilMembers council);
        Task<object> GetCouncilbyEmailmember(string email, int council);

    }
}