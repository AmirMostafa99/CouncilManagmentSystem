using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationServies _notificationServies;
        private readonly IUserServies _userServies;

        public NotificationController(INotificationServies notificationServies, IUserServies userServies)
        {
            _notificationServies = notificationServies;
            _userServies = userServies;
        }
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllNotificationForUser()
        {
            // var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = "Mariam.20375785@compit.aun.edu.eg";
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
       // [Authorize]
        [HttpPut]
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

    }
}
