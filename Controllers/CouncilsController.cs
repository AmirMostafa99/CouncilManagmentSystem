using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.notfication;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Xml.Linq;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouncilsController : ControllerBase
    {
        private readonly ICouncilsServies _councilServies;
        private readonly ICouncilMembersServies _councilMembersServies;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserServies _userServies;
        private readonly ITypeCouncilServies _typecouncilservies;
        private readonly INotificationServies _notificationServies;

        public CouncilsController(ICouncilsServies councilServies, ICouncilMembersServies councilMembersServies, IHubContext<NotificationHub> hubContext, ApplicationDbContext dbContext, IUserServies userServies, ITypeCouncilServies typecouncilservies, INotificationServies notificationServies)
        {
            _councilServies = councilServies;
            _councilMembersServies = councilMembersServies;
            _hubContext = hubContext;
            _dbContext = dbContext;
            _userServies = userServies;
            _typecouncilservies = typecouncilservies;
            _notificationServies = notificationServies;
        }
        [Authorize]
        [Authorize(Policy = "RequireAddCouncilPermission")]
        [HttpPost(template: "CreateCouncil")]
        public async Task<IActionResult> createcouncil([FromBody] AddCouncilsDTO DTO)
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userEmail == null)
            {
                return BadRequest("Please Login");
            }

            var user = await _userServies.getuserByEmail(userEmail);
            if (user == null)
            {
                return BadRequest("not found");
            }
            var typecounill = await _typecouncilservies.GetUserOfTypeCouncil(user.Id);
            if (typecounill == null)
            {
                return BadRequest("errrrrror");
            }
            if (ModelState.IsValid)
            {
                var council = new Councils
                {
                    Title = DTO.Title,
                    Date = DTO.Date,
                    HallId = DTO.HallId,
                    TypeCouncilId = typecounill.Id
                };
                var councilres = await _councilServies.AddCouncil(council);
                return Ok(councilres);

            }
            return BadRequest("you have wrong in your data. ");
        }

        [Authorize]
        [HttpGet(template: "GetAllCouncils")]
        public async Task<IActionResult> getallcouncils()
        {
            var councils = await _councilServies.GetallCouncils();
            return Ok(councils);
        }

        [Authorize]
        [HttpGet(template: "GetCouncilById")]
        public async Task<IActionResult> getcouncilbyid(int id)
        {
            var council = await _councilServies.GetCouncilById(id);
            return Ok(council);
        }
        [Authorize]
        [HttpGet(template: "GetAllCouncilsByIdType")]
        public async Task<IActionResult> getallcouncilsbyidtype(int typeid)
        {
            if (ModelState.IsValid)
            {
                var councils = await _councilServies.GetAllCouncilsByIdType(typeid);
                return Ok(councils);
            }
            return BadRequest("you have wrong in your data. ");
        }
        [Authorize]
        [HttpGet(template: "GetAllCouncilsByIdHall")]
        public async Task<IActionResult> getalcouncilsbyidhall(int hallid)
        {
            if (ModelState.IsValid)
            {
                var councils = await _councilServies.GetCouncilSbyIDhalls(hallid);
                return Ok(councils);
            }
            return BadRequest("you have wrong in your data. ");
        }
        [Authorize]
        [HttpGet(template: "GetAllCouncilByname")]
        public async Task<IActionResult> getallcouncilsbyname(string name)
        {

            if (ModelState.IsValid)
            {
                //token
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userEmail == null)
                {
                    return BadRequest("not authorization");
                }
                var user = await _userServies.getuserByEmail(userEmail);
                if (user == null)
                {
                    return NotFound("Your Email not found");
                }

                var councils = await _councilServies.GetCouncilsByTitle(name);
                var councilmembers = new List<Object>();
                foreach (var councill in councils)
                {
                    var councilmem = await _councilMembersServies.GetcouncilmemberlById(councill.Id, user.Id);
                    if (councilmem != null)
                    {
                        if (councilmem.IsAttending == true)
                        {
                            var hall = await _dbContext.Halls.FirstOrDefaultAsync(x => x.Id == councill.HallId);
                            if (hall != null)
                            {
                                var ob = new { councilName = councill.Title, Date = councill.Date, Hall = hall.Name };
                                councilmembers.Add(ob);
                            }
                        }

                    }
                }
                return Ok(councilmembers);
            }
            return BadRequest("you have wrong in your data. ");
        }
        [Authorize]
        [HttpGet(template: "GetCouncilBydate")]
        public async Task<IActionResult> getCouncilbydate(DateTime date)
        {

            if (ModelState.IsValid)
            {
                var councils = await _councilServies.GetCouncilByDate(date);
                return Ok(councils);
            }
            return BadRequest("you have wrong in your data. ");
        }


        [Authorize]
        [Authorize(Policy = "RequireEditCouncilPermission")]
        [HttpPut(template: "UpdateCouncil")]
        public async Task<IActionResult> updatecouncil(int id, [FromForm] AddCouncilsDTO DTO)
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userEmail == null)
            {
                return BadRequest("Please Login");
            }

            var user1 = await _userServies.getuserByEmail(userEmail);
            if (user1 == null)
            {
                return BadRequest("not found");
            }
            var typecounill = await _typecouncilservies.GetUserOfTypeCouncil(user1.Id);
            if (typecounill == null)
            {
                return BadRequest("errrrrror");
            }


            var council = await _councilServies.GetCouncilById(id);
            if (council == null)
            {
                return NotFound($"You have error in your data ");
            }
            council.Title = DTO.Title;
            council.Date = DTO.Date;
            council.TypeCouncilId = typecounill.Id;
            council.HallId = DTO.HallId;
            var councilres = await _councilServies.UpdateCouncil(council);


            var members = await _councilMembersServies.GetAllIDMembersbyidCouncil(id);
            if (members == null)
            {
                return Ok();
            }
            var hall = await _dbContext.Halls.FirstOrDefaultAsync(x => x.Id == council.HallId);
            if (hall == null)
            {
                return NotFound($"You have error in your data ");
            }
            foreach (var user in members)
            {
                if (user.IsAttending == true)
                {
                    var not = new Notifications
                    {
                        CouncilId = council.Id,
                        MemberId = user.MemberId,
                        IsSeen = false,

                    };
                    await _notificationServies.AddNotifcation(not);
                    await _hubContext.Clients.User(user.MemberId.ToString()).SendAsync("ReceiveNotification", not);
                }
            }
            return Ok(councilres);
        }
       // [Authorize]
        [HttpGet(template: "GetAllCouncilByDate")]
        public async Task<IActionResult> GetAllCouncilByDate(DateTime date)
        {
            //var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = "Mariam.20375785@compit.aun.edu.eg";
            if (userEmail == null)
            {
                return NotFound("not authorize");
            }
            var user = await _userServies.getuserByEmail(userEmail);
            if (user == null)
            {
                return NotFound("Your date have error");
            }

            var councils = await _councilServies.GetCouncilSbyDate(date);
            var councilmembers = new List<Object>();
            foreach (var councill in councils)
            {
                var councilmem = await _councilMembersServies.GetcouncilmemberlById(councill.Id, user.Id);
                if (councilmem != null)
                {
                    if (councilmem.IsAttending == true)
                    {
                        var hall = await _dbContext.Halls.FirstOrDefaultAsync(x => x.Id == councill.HallId);
                        if (hall != null)
                        {
                            var ob = new { councilName = councill.Title, Date = councill.Date, Hall = hall.Name };
                            councilmembers.Add(ob);
                        }
                    }

                }

            }
            return Ok(councilmembers);




        }
      
    }
}
