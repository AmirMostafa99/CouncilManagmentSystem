using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Claims;
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

        //  [Authorize]
        // [Authorize(Policy = "RequireAddCouncilPermission")]
        [HttpPost("AddCouncilMember")]
        public async Task<IActionResult> AddCouncilMember([FromBody] AddCouncilmemberDTO dto)
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
                //string uploadsPath = Path.Combine(_environment.ContentRootPath, "uploadsPDF");
                //if (!Directory.Exists(uploadsPath))
                //{
                //    Directory.CreateDirectory(uploadsPath);
                //}
                //string fileName = Path.GetFileName(dto.Pdf.FileName);
                //string filePath = Path.Combine(uploadsPath, fileName);
                //var file = "";
                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await dto.Pdf.CopyToAsync(stream);
                //    file = fileName;
                //}
                var councilMember = new CouncilMembers
                {
                    CouncilId = dto.CouncilId,
                    MemberId = user.Id
                };
                await _councilMemberService.Addmember(councilMember);
            }
            return Ok();
        }
        [Authorize]
        [HttpPut(template: "Confirm attendance")]
        public async Task<IActionResult> ConfirmAttendance([FromForm] AddConfirmAttendanceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userService.getuserByEmail(userEmail);
            if (user == null)
            {
                return NotFound($"you have error in your data");
            }
            var councilMember = await _councilMemberService.GetcouncilmemberlById(dto.CouncilId, user.Id);
            if (councilMember == null)
            {
                return NotFound($"you have error in your data");
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
            councilMember.CouncilId= dto.CouncilId;
            councilMember.Pdf = file;
            councilMember.IsAttending = dto.IsAttending;
            councilMember.ReasonNonAttendance= dto.ReasonNonAttendance;
            
           var cou= _councilMemberService.updatecouncilmember(councilMember);

            return Ok(cou);
        }



        [Authorize]
        [Authorize(Policy = "RequireAddCouncilPermission")]
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


        [HttpGet(template: "GetAllUserInCounilType")]
        public async Task<IActionResult> getAllUserInCounilType(int idtypecouncil)
        {
            var users = await _councilMemberService.getAllUserInDep(idtypecouncil);
            return Ok(users);
        }

    }
}