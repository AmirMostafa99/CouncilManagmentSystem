using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly ICollageServies _collageServies;
        private readonly IDepartmentServies _departmentServies;

        public DepartmentController(IDepartmentServies departmentServies, ICollageServies collageServies)
        {
            _departmentServies = departmentServies;
            _collageServies = collageServies;
        }
        //Add department
        [HttpPost(template: "AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromForm] AddDepartmntDto dto)
        {
            if (ModelState.IsValid)
            {
                var department = new Department { name = dto.name, collage_id = dto.collage_id };

                await _departmentServies.AddDepartment(department);
                return Ok(department);
            }
            return BadRequest("you have wrong in your data. ");
        }

        //get all departments
        [HttpGet(template: "GetAllDepartments")]
        public async Task<IActionResult> getalldepartment()
        {
            var departments = await _departmentServies.getAlldepartment();
            return Ok(departments);
        }

        //update department
        [HttpPut(template: "Update Department")]
        public async Task<IActionResult> updatedepartment(int id, AddDepartmntDto dto)
        {
            var dep = await _departmentServies.GetDepartmentById(id);
            if (dep == null)
            {
                return NotFound("Not found this department");
            }

            dep.name = dto.name;
            dep.collage_id = dto.collage_id;

            _departmentServies.UpdateDepartment(dep);
            return Ok(dep);
        }

        ////delete department
        //[HttpDelete(template:"delete department")]
        //public async Task<IActionResult> deletedepartment(int id)
        //{
        //    var dep = await _departmentServies.GetDepartmentById(id);
        //    if (dep == null)
        //    {
        //        return NotFound("Not found this department");
        //    }
        //    _departmentServies.DeleteDepartment(dep);
        //    return Ok(dep);
        //}


        ////Gett all department for id_collage
        [HttpGet(template: "GetallDepartmentByCollageId")]
        public async Task<IActionResult> getAllDepByCollageId(int id)
        {
            if (ModelState.IsValid)
            {
                var departments = await _departmentServies.get_dep_byIDCollage(id);
                return Ok(departments);
            }
            return BadRequest("you have wrong in your data. ");
        }



    }
}
