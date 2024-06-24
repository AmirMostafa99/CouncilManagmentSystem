using Microsoft.AspNetCore.SignalR;

namespace CouncilsManagmentSystem.notfication
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string MeetingName, DateTime dateTime)
        {
            await Clients.All.SendAsync("ReceiveNotification", MeetingName, dateTime);
        }
    }
}


