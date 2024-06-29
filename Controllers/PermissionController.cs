using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Migrations;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionsServies _permissionsServies;
        private readonly IUserServies _userServies;

        public PermissionController(IPermissionsServies permissionsServies , IUserServies userservies)
        {
            _userServies = userservies;
            _permissionsServies = permissionsServies;
        }

        [Authorize]
        [Authorize(Policy = "RequireUpdatepermission")]
        [HttpPut(template: "Updatepermission")]
        public async Task<IActionResult> addwithpermissions([FromBody] AddPermissionsDTO dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userServies.getuserByEmail(dto.Email);
                if(user==null)
                {
                    return BadRequest("This email is not found!!");
                }
                var userPerm = await _permissionsServies.getpermissionByid(user.Id);
              

                if (userPerm == null)
                {
                    var permission = new Permissionss
                    {
                        userId = user.Id,
                        AddCouncil = dto.AddCouncil,
                        EditCouncil = dto.EditCouncil,
                        CreateTypeCouncil = dto.CreateTypeCouncil,
                        EditTypeCouncil = dto.EditTypeCouncil,
                        AddMembersByExcil = dto.AddMembersByExcil,
                        AddMembers = dto.AddMembers,
                        AddTopic = dto.AddTopic,
                        Arrange = dto.Arrange,
                        AddResult = dto.AddResult,
                        AddDepartment = dto.AddDepartment,
                        AddCollage = dto.AddCollage,
                        Updatepermission = dto.Updatepermission,
                        DeactiveUser = dto.DeactiveUser,
                        UpdateUser = dto.UpdateUser,
                        AddHall = dto.AddHall

                    };

                  var newper=  await _permissionsServies.Addpermission(permission);
                    return Ok(newper);
                }
                else
                {
                    userPerm.AddCouncil = dto.AddCouncil;
                    userPerm.EditCouncil = dto.EditCouncil;
                    userPerm.CreateTypeCouncil = dto.CreateTypeCouncil;
                    userPerm.EditTypeCouncil = dto.EditTypeCouncil;
                    userPerm.AddMembersByExcil = dto.AddMembersByExcil;
                    userPerm.AddMembers = dto.AddMembers;
                    userPerm.AddTopic = dto.AddTopic;
                    userPerm.Arrange = dto.Arrange;
                    userPerm.AddResult = dto.AddResult;
                    userPerm.AddDepartment = dto.AddDepartment;
                    userPerm.AddCollage = dto.AddCollage;
                    userPerm.Updatepermission = dto.Updatepermission;
                    userPerm.DeactiveUser = dto.DeactiveUser;
                    userPerm.UpdateUser = dto.UpdateUser;
                    userPerm.AddHall = dto.AddHall;
                    var newper = await _permissionsServies.UpdatePermission(userPerm);
                      return Ok(newper);

                }
            }
            return BadRequest("you have wrong in your data. ");
        }

        [Authorize]
        [HttpGet(template:"GetPermissionsUser")]
        public async Task<IActionResult> getpermissionuser(string email)
        {
            var user = await _userServies.getuserByEmail(email);
            if (user == null)
            {
                return BadRequest("This email is not found!!");
            }
            var per = await _permissionsServies.getObjectpermissionByid(user.Id);
            if(per == null)
            {
                return BadRequest("you have wrong in your data. ");
            }
            return Ok(per);
        }

        [Authorize]
        [HttpGet(template: "GetPermissionsUserByToken")]
        public async Task<IActionResult> GetPermissionsUserByToken()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userServies.getuserByEmail(userEmail);
            if (user == null)
            {
                return BadRequest("This email is not found!!");
            }
            var per = await _permissionsServies.getObjectpermissionByid(user.Id);
            if (per == null)
            {
                return BadRequest("you have wrong in your data. ");
            }
            return Ok(per);
        }

    }
}
