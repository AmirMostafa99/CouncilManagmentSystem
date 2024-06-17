using CouncilsManagmentSystem.Models;

namespace CouncilsManagmentSystem.Services
{
        public interface ICollageServies
        {
            Task<Collage> GetCollageByid(int id);
            Task<Collage> Addcollages(Collage collage);
            Task<Collage> GetCollageByName(string name);
            Task<IEnumerable<Collage>> getAllcollages();
            Collage updatecollage(Collage collage);
            Collage Deletecollage(Collage collage);
        }
  
}
