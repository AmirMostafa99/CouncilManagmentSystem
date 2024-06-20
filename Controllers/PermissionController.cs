using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionsServies _permissionsServies;

        public PermissionController(IPermissionsServies permissionsServies)
        {
            _permissionsServies = permissionsServies;
        }

        [HttpPost("addwithpermissions")]
        public async Task<IActionResult> addwithpermissions([FromBody] AddPermissionsDTO dto)
        {
            if (ModelState.IsValid)
            {
                var permission = new Permissionss {
                    userId = dto.UserId,
                    AddCouncil = dto.AddCouncil,
                    EditCouncil = dto.EditCouncil,
                    CreateTypeCouncil =dto.CreateTypeCouncil,
                    EditTypeCouncil = dto.EditTypeCouncil,
                    AddMembersByExcil = dto.AddMembersByExcil,
                    AddMembers = dto.AddMembers,
                    AddTopic = dto.AddTopic,
                    Arrange = dto.Arrange,
                    AddResult = dto.AddResult
                };

                await _permissionsServies.Addpermission(permission);
                return Ok(permission);
            }
            return BadRequest("you have wrong in your data. ");
        }
    }
}
