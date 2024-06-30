using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.notfication;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICouncilsServies _councilServies;
        private readonly ApplicationDbContext _dbcontext;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationServies _notificationServies;

        public CouncilMemberController(
            ICouncilMembersServies councilMemberService,
            IConfiguration configuration,
            IMailingService mailingService,
            IUserServies userService,
            IWebHostEnvironment environment,
            ICouncilsServies councilServies,
            ApplicationDbContext dbcontext,
            IHubContext<NotificationHub> hubContext,
            INotificationServies notificationServies)
        {
            _councilMemberService = councilMemberService;
            _userService = userService;
            _configuration = configuration;
            _mailingService = mailingService;
            _environment = environment;
            _councilServies = councilServies;
            _dbcontext = dbcontext;
            _hubContext = hubContext;
            _notificationServies = notificationServies;
        }

        [Authorize]
         [Authorize(Policy = "RequireAddCouncilPermission")]
        [HttpPost("AddCouncilMember")]
        public async Task<IActionResult> AddCouncilMember([FromBody] AddCouncilmemberDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (_councilServies == null)
            {
                return StatusCode(500, "Council services not initialized.");
            }

            var council1 = await _councilServies.GetCouncilById(dto.CouncilId);
            if (council1 == null)
            {
                return NotFound("You have an error in your data.");
            }


            if (_userService == null)
            {
                return StatusCode(500, "User services not initialized.");
            }


            if (_councilMemberService == null)
            {
                return StatusCode(500, "Council member services not initialized.");
            }


            if (_hubContext == null)
            {
                return StatusCode(500, "Notification hub context not initialized.");
            }

            foreach (var email in dto.EmailUsers)
            {
                var user = await _userService.getuserByEmail(email);

                if (user == null)
                {
                    return NotFound($"User with email {email} not found.");
                }

                var councilMember = new CouncilMembers
                {
                    CouncilId = dto.CouncilId,
                    MemberId = user.Id
                };
                var check = await _councilMemberService.GetcouncilmemberlById(dto.CouncilId, user.Id);
                if(check != null)
                {
                    continue;
                }

                try
                {
                    await _councilMemberService.Addmember(councilMember);
                    var hall = await _dbcontext.Halls.FirstOrDefaultAsync(x => x.Id == council1.HallId);
                    if (hall == null)
                    {
                        return NotFound($"You have error in your data ");
                    }
                    var not = new Notifications
                    {
                        CouncilId=council1.Id,
                        MemberId=user.Id,
                        IsSeen = false,

                    };
                     await _notificationServies.AddNotifcation(not);
                    await _hubContext.Clients.User(user.Id.ToString()).SendAsync("ReceiveNotification", not);
                   
                    
                   
                }
                catch (Exception ex)
                {

                    return StatusCode(500, $"An error occurred while adding the council member: {ex.Message}");
                }
            }

            return Ok("Council member(s) added successfully.");
        }
        [Authorize]
        [HttpPut(template: "Confirm attendance")]
        public async Task<IActionResult> ConfirmAttendance([FromBody] AddConfirmAttendanceDto dto)
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
            councilMember.CouncilId= dto.CouncilId;
          //  councilMember.Pdf = file;
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



        [Authorize]
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



        [Authorize]
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
        [Authorize]
        [HttpDelete(template: "delete")]
        public async Task<IActionResult> delete(int councilId, string email)
        {
            var user = await _userService.getuserByEmail(email);
            var councilmem = await _councilMemberService.GetcouncilmemberlById(councilId, user.Id);
            var res = _councilMemberService.delete(councilmem);
            return Ok(res);

        }

        [Authorize]
        [HttpGet(template: "ISValidInThisCouncil")]
        public async Task<IActionResult> Checkcouncilbyidmember(string email, int idcouncil)
        {
            var isvalid = await _councilMemberService.GetCouncilbyEmailmember(email, idcouncil);
            return Ok(isvalid);

        }

        [Authorize]
        [HttpGet(template: "GetAllUserInCounilType")]
        public async Task<IActionResult> getAllUserInCounilType(int idtypecouncil)
        {
            var users = await _councilMemberService.getAllUserInDep(idtypecouncil);
            return Ok(users);
        }

        [Authorize]
        [HttpGet(template: "GetAllReasonsForNonAttendance")]
        public async Task<IActionResult> GetAllReasonsForNonAttendance(int councilId)
        {
            var council=await _councilServies.GetCouncilById(councilId);
            if(council==null)
            {
                return NotFound("This council Not Found");
            }
            var reasons = await _councilMemberService.GetAllCouncilMemberIsNotAtt(councilId);
            return Ok(reasons);
        }

       
    }
}