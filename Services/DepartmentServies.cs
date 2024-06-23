using CouncilsManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace CouncilsManagmentSystem.Services
{
    public class DepartmentServies : IDepartmentServies
    {
        private readonly ApplicationDbContext _context;

        public DepartmentServies(ApplicationDbContext context)
        {
            _context = context;
        }
        //add
        public async Task<Department> AddDepartment(Department department)
        {
            await _context.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }
        //servies of delete
        public Department DeleteDepartment(Department department)
        {
            _context.Remove(department);
            _context.SaveChangesAsync();
            return department;
        }
        //get list
        public async Task<IEnumerable<object>> getAlldepartment()
        {
            var departments = await _context.departments.OrderBy(x => x.name).Include(z => z.Collage).Select(z => new { ID = z.id, Name = z.name, collagename = z.Collage.Name }).ToListAsync();
            return departments;
        }

        public async Task<object> GetCollagetByIdDep(int id)
        {
            var collage1 = await _context.departments.Where(x => x.id == id).Include(a => a.Collage).Select(a => new { Collage = a.Collage.Name }).FirstOrDefaultAsync();
            return collage1.Collage;
        }

        //get id
        public async Task<Department> GetDepartmentById(int id)
        {
            var department = await _context.departments.FirstOrDefaultAsync(x => x.id == id);
            return department;
        }
        //det list of

        public async Task<IEnumerable<Department>> get_dep_byIDCollage(int id)
        {
            var department = await _context.departments.Where(x => x.collage_id == id).ToListAsync();
            return department;
        }
        //get one item
        public async Task<Department> Get_dep_idcollage(int id_collage, string name_depart)
        {
            var department = await _context.departments.FirstOrDefaultAsync(x => x.collage_id == id_collage && x.name == name_depart);
            return department;
        }

        //servies of update
        public Department UpdateDepartment(Department department)
        {
            _context.Update(department);
            _context.SaveChanges();
            return department;
        }
    }

}
