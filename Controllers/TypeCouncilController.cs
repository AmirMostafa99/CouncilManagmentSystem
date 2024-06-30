using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeCouncilController : ControllerBase
    {
        private readonly IUserServies _userServies;

        private readonly IDepartmentServies _departmentServies;
        private readonly ITypeCouncilServies _typeCouncilServies;

        public readonly ICollageServies _collageServies;
        public TypeCouncilController(ITypeCouncilServies typeCouncilServies, ICollageServies collageServies, IDepartmentServies departmentServies, IUserServies userServies)
        {
            _userServies = userServies;

            _collageServies = collageServies;
            _departmentServies = departmentServies;
            _typeCouncilServies = typeCouncilServies;
        }

        [Authorize]
        [Authorize(Policy = "RequireCreateTypeCouncilPermission")]
        [HttpPost(template: "AddTypeCouncil")]
        public async Task<IActionResult> addTypeCouncil([FromBody] AddTypeCouncilsDTO type)
        {

            if (ModelState.IsValid)
            {
                var department = await _departmentServies.GetDepartmentById(type.DepartmentId);
                if (department == null)
                {
                    return BadRequest("We have this council !");
                }
                var charman = await _userServies.getuserByEmail(type.ChairmanCouncilEmail);
                var Secretary = await _userServies.getuserByEmail(type.SecretaryCouncilEmail);
                if (charman == null && Secretary == null)
                {
                    return BadRequest("We donot have this persons !");
                }
                var newtype = new TypeCouncil
                {
                    Name = type.Name,
                    ChairmanCouncilId = charman.Id,
                    SecretaryCouncilId = Secretary.Id,
                    DepartmentId = type.DepartmentId

                };
                await _typeCouncilServies.checkuser(newtype);
                var type1 = await _typeCouncilServies.addtypecouncil(newtype);

                return Ok(type1);
            }
            return BadRequest("There is an error in your data.");
        }
        [Authorize]
        [HttpGet(template: "Get All types")]
        public async Task<IActionResult> getalltypes()
        {

            var types = await _typeCouncilServies.listtypecouncil();
            return Ok(types);

        }
        [Authorize]
        [HttpGet(template: "Get type by id")]
        public async Task<IActionResult> getTypeById(int id)
        {
            var type = await _typeCouncilServies.GetCouncilById(id);
            //var user = await _userServies.getuserByid(type.ChairmanCouncilId);
            return Ok(type);
        }
        [Authorize]
        [HttpGet("Get Chairnam of council")]
        public async Task<IActionResult> getchairman(int idCouncil)
        {
            if (ModelState.IsValid)
            {
                var type = await _typeCouncilServies.GetCouncilById(idCouncil);
                var user = await _userServies.getuserByid(type.ChairmanCouncilId);
                return Ok(user);

            }
            return BadRequest("There is an error in your data.");
        }
        [Authorize]

        [HttpGet("Get Secretary of council")]
        public async Task<IActionResult> getSecretary(int idCouncil)
        {
            if (ModelState.IsValid)
            {
                var type = await _typeCouncilServies.GetCouncilById(idCouncil);
                var user = await _userServies.getuserByid(type.SecretaryCouncilId);
                return Ok(user);

            }
            return BadRequest("There is an error in your data.");
        }



        [Authorize]
        [Authorize(Policy = "RequireEditTypeCouncilPermission")]
        [HttpPut(template: "UpdateType")]
        public async Task<IActionResult> updatetypecouncil(int id, [FromForm] AddTypeCouncilsDTO type)
        {
            var type1 = await _typeCouncilServies.GetCouncilById(id);
            var charman = await _userServies.getuserByEmail(type.ChairmanCouncilEmail);
            var Secretary = await _userServies.getuserByEmail(type.SecretaryCouncilEmail);

            type1.Name = type.Name;
            type1.ChairmanCouncilId = charman.Id;
            type1.SecretaryCouncilId = Secretary.Id;
            type1.DepartmentId = type.DepartmentId;

            await _typeCouncilServies.checkuser(type1);

            var type2 = _typeCouncilServies.updateTypeCouncilAsync(type1);
            type2 = await _typeCouncilServies.GetCouncilById(type2.Id);
            return Ok(type2);
        }


    }

}
