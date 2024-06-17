namespace CouncilsManagmentSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using CouncilsManagmentSystem.Models;
public class CollageServies : ICollageServies
{
    private readonly ApplicationDbContext _context;

    public CollageServies(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Collage> GetCollageByid(int id)
    {
        var collage = _context.collages.FirstOrDefault(x => x.Id == id);
        return collage;
    }

    public async Task<Collage> Addcollages(Collage collage)
    {
        await _context.AddAsync(collage);
        await _context.SaveChangesAsync();
        return collage;
    }

    public async Task<Collage> GetCollageByName(string name)
    {
        var collage = await _context.collages.SingleOrDefaultAsync(x => x.Name == name);
        return collage;
    }


    public Collage updatecollage(Collage collage)
    {

        _context.Update(collage);
        _context.SaveChanges();

        return collage;

    }

    public Collage Deletecollage(Collage collage)
    {
        _context.Remove(collage);
        _context.SaveChanges();
        return collage;
    }

    public async Task<IEnumerable<Collage>> getAllcollages()
    {
        var collages = await _context.collages.OrderBy(x => x.Name).ToListAsync();
        return collages;
    }
}

