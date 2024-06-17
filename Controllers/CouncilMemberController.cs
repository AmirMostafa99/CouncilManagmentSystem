﻿using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouncilMemberController : ControllerBase
    {
        private readonly ICouncilMembersServies _councilMemberService;
        private readonly IUserServies _userService;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IMailingService _mailingService;

        public CouncilMemberController(
            ICouncilMembersServies councilMemberService,
            IConfiguration configuration,
            IMailingService mailingService,
            IUserServies userService,
            IWebHostEnvironment environment)
        {
            _councilMemberService = councilMemberService;
            _userService = userService;
            _configuration = configuration;
            _mailingService = mailingService;
            _environment = environment;
        }


        [HttpPost("AddCouncilMember")]
        public async Task<IActionResult> AddCouncilMember([FromForm] AddCouncilmemberDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var email in dto.EmailUsers)
            {
                var user = await _userService.getuserByEmail(email);
                if (user == null)
                {
                    return NotFound($"User with email {email} not found.");
                }
                string uploadsPath = Path.Combine(_environment.ContentRootPath, "uploadsPDF");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }
                string fileName = Path.GetFileName(dto.Pdf.FileName);
                string filePath = Path.Combine(uploadsPath, fileName);
                var file = "";
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Pdf.CopyToAsync(stream);
                    file = fileName;
                }
                var councilMember = new CouncilMembers
                {
                    IsAttending = dto.IsAttending,
                    ReasonNonAttendance = dto.ReasonNonAttendance,
                    Pdf = file,
                    CouncilId = dto.CouncilId,
                    MemberId = user.Id
                };
                await _councilMemberService.Addmember(councilMember);
            }
            return Ok();
        }






        [HttpGet(template: "GetAllMembersByIdCouncil")]
        public async Task<IActionResult> GetAllMembersByIdCouncil(int idcouncil)
        {
            var members = await _councilMemberService.GetAllMembersbyidCouncil(idcouncil);
            if (members.Any())
            {
                return Ok(members);
            }
            return NotFound();
        }




        [HttpGet(template: "GetAllCouncilsbyidmember")]
        public async Task<IActionResult> GetAllCouncilsbyidmember(string iduser)
        {
            var council = await _councilMemberService.GetAllCouncilsbyidmember(iduser);
            if (council.Any())
            {
                return Ok(council);
            }
            return NotFound();
        }




        [HttpGet(template: "GetAllCouncilsbyEmailmember")]
        public async Task<IActionResult> GetAllCouncilsbyEmailmember(string email)
        {
            var council = await _councilMemberService.GetAllCouncilsbyEmailmember(email);
            if (council.Any())
            {
                return Ok(council);
            }
            return NotFound();
        }

        [HttpDelete(template: "delete")]
        public async Task<IActionResult> delete(int councilId, string email)
        {
            var user = await _userService.getuserByEmail(email);
            var councilmem = await _councilMemberService.GetcouncilmemberlById(councilId, user.Id);
            var res = _councilMemberService.delete(councilmem);
            return Ok(res);

        }
        [HttpGet(template: "ISValidInThisCouncil")]
        public async Task<IActionResult> Checkcouncilbyidmember(string email, int idcouncil)
        {
            var isvalid = await _councilMemberService.GetCouncilbyEmailmember(email, idcouncil);
            return Ok(isvalid);

        }

    }
}