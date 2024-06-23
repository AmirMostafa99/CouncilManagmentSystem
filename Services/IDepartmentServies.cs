using CouncilsManagmentSystem.Models;

namespace CouncilsManagmentSystem.Services
{
    public interface IDepartmentServies
    {
        Task<Department> GetDepartmentById(int id);
        Task<Department> AddDepartment(Department department);
        Task<Department> Get_dep_idcollage(int id_collage, string name_depart);
        Task<IEnumerable<object>> getAlldepartment();
        Department UpdateDepartment(Department department);
        Department DeleteDepartment(Department department);
        Task<IEnumerable<Department>> get_dep_byIDCollage(int id);
        Task<object> GetCollagetByIdDep(int id);

    }
}
