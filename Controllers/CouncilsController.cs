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

        public CouncilsController(ICouncilsServies councilServies, ICouncilMembersServies councilMembersServies, IHubContext<NotificationHub> hubContext, ApplicationDbContext dbContext)
        {
            _councilServies = councilServies;
            _councilMembersServies = councilMembersServies;
            _hubContext = hubContext;
            _dbContext = dbContext;
        }
        [Authorize]
        [Authorize(Policy = "RequireAddCouncilPermission")]
        [HttpPost(template: "CreateCouncil")]
        public async Task<IActionResult> createcouncil([FromBody] AddCouncilsDTO DTO)
        {
            if (ModelState.IsValid)
            {
                var council = new Councils
                {
                    Title = DTO.Title,
                    Date = DTO.Date,
                    HallId = DTO.HallId,
                    TypeCouncilId = DTO.TypeCouncilId
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
                var councils = await _councilServies.GetCouncilsByTitle(name);
                return Ok(councils);
            }
            return BadRequest("you have wrong in your data. ");
        }
        [Authorize]
        [HttpGet(template: "GetCouncilBydate")]
        public async Task<IActionResult> getCouncilbydate(DateTime date)
        {
            //if (date.Year < DateTime.MinValue.Year || date.Month < 1 || date.Month > 12 || date.Day < 1 || date.Day > DateTime.DaysInMonth(date.Year, date.Month))
            if (ModelState.IsValid)
            {
                var councils = await _councilServies.GetCouncilByDate(date);
                return Ok(councils);
            }
            return BadRequest("you have wrong in your data. ");
        }


       // [Authorize]
        //[Authorize(Policy = "RequireEditCouncilPermission")]
        [HttpPut(template: "UpdateCouncil")]
        public async Task<IActionResult> updatecouncil(int id,[FromForm] AddCouncilsDTO DTO)
        {
            var council = await _councilServies.GetCouncilById(id);
            if (council == null)
            {
                return NotFound($"You have error in your data ");
            }
            council.Title = DTO.Title;
            council.Date = DTO.Date;
            council.TypeCouncilId = DTO.TypeCouncilId;
            council.HallId = DTO.HallId;
            var councilres = await _councilServies.UpdateCouncil(council);
            var members = await _councilMembersServies.GetAllIDMembersbyidCouncil(id);
            if (members == null)
            {
                return Ok(councilres);
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
                    await _hubContext.Clients.User(user.MemberId).SendAsync("ReceiveNotification", council.Title, hall.Name, council.Date);
                }
            }
            return Ok(councilres);
        }




    }
}
