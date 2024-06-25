using CouncilsManagmentSystem.Models;
using Microsoft.AspNetCore.SignalR;

namespace CouncilsManagmentSystem.notfication
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string Id, object council)
        {
            // await Clients.All.SendAsync("ReceiveNotification", MeetingName, dateTime);
            await Clients.User(Id).SendAsync("ReceiveNotification", council);
        }
    }
}

