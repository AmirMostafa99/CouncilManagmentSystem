using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationServies _notificationServies;
        private readonly IUserServies _userServies;
        private readonly ICouncilsServies _councilsServies;
        private readonly ApplicationDbContext _dbContext;

        public NotificationController(INotificationServies notificationServies, IUserServies userServies, ICouncilsServies councilsServies, ApplicationDbContext dbContext)
        {
            _notificationServies = notificationServies;
            _userServies = userServies;
            _councilsServies = councilsServies;
            _dbContext = dbContext;
        }
        [Authorize]
        [HttpGet(template: "GetAllNotificationForUser")]
        public async Task<IActionResult> GetAllNotificationForUser()
        {
             var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(userEmail == null)
            {
                return BadRequest(" Unauthorize");
            }
            var user = await _userServies.getuserByEmail(userEmail);
            if(user == null)
            {
                return BadRequest("Not Found This User");
            }
            var not = await _notificationServies.GetAllNotifcationByIdUser(user.Id);
            return Ok(not);

        }
        [Authorize]
        [HttpPut(template: "UpdateNotification")]
        public async Task<IActionResult> UpdateNotification(int NotId , bool IsSeen)
        {
            var not = await _notificationServies.GetNotifcationById(NotId);
            if(not==null)
            {
                return NotFound("Not Found This Notification");
            }
            not.IsSeen= IsSeen;

            _notificationServies.UpdateNotifcation(not);

            return Ok(not);

        }

        [Authorize]
        [HttpGet(template: "GetDataofcouncilNotification")]
        public async Task<IActionResult> GetDataofcouncilNotification(int idNot)
        {
            var Not = await _notificationServies.GetNotifcationById(idNot);
            if(Not!=null)
            {
                var coun = await _councilsServies.GetCouncilById(Not.CouncilId);
                if(coun !=null)
                {
                    var hall = await _dbContext.Halls.FirstOrDefaultAsync(x => x.Id == coun.HallId);
                    if(hall!=null)
                    {
                        var ob = new { 
                        Idmember=Not.CouncilMembers,
                        councilName=coun.Title,
                        Date=coun.Date,
                        Hall=hall.Name,
                        TypeCouncil=coun.TypeCouncilId,
                        counilId=coun.Id
                        
                        };
                        return Ok(ob);
                    }
                }
                
            }
            return Ok();
        }

    }
}
