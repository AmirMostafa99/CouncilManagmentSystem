using CouncilsManagmentSystem.Models;

namespace CouncilsManagmentSystem.Services
{
    public interface ICouncilsServies
    {
        Task<IEnumerable<Councils>> GetallCouncils();
        Task<object> AddCouncil(Councils council);
        Task<Councils> GetCouncilById(int councilId);
        Task<IEnumerable<Object>> GetAllCouncilsByIdType(int typeId);
        Task<Councils> GetCouncilByDate(DateTime date); 
        Task<IEnumerable<Councils>> GetCouncilsByTitle (string name);
        Task<string> UpdateCouncil (Councils council);
        Task<IEnumerable<Councils>> GetCouncilSbyIDhalls(int Idhall);
        Task<IEnumerable<Councils>> GetCouncilSbyDate(DateTime date);
        Task<IEnumerable<Object>> GetAllLastCouncilsByIdType(int typeId);

    }
}
