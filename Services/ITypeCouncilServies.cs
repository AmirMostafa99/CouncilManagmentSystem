using CouncilsManagmentSystem.Models;

namespace CouncilsManagmentSystem.Services
{
    public interface ITypeCouncilServies
    {
        Task<TypeCouncil> GetCouncilById(int id);
        Task<TypeCouncil> addtypecouncil(TypeCouncil typeCouncil);
        Task<TypeCouncil> checkuser(TypeCouncil typeCouncil);

        TypeCouncil updateTypeCouncilAsync(TypeCouncil typeCouncil);
        Task<IEnumerable<TypeCouncil>> listtypecouncil();
        Task<TypeCouncil> GetUserOfTypeCouncil(string iduser);

    }
}
