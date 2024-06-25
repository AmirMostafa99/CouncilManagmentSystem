using CouncilsManagmentSystem.Models;
using Microsoft.AspNetCore.SignalR;

namespace CouncilsManagmentSystem.notfication
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string Id, string councilname, DateTime date, string Hallname)
        {
            // await Clients.All.SendAsync("ReceiveNotification", MeetingName, dateTime);
            await Clients.User(Id).SendAsync("ReceiveNotification", councilname, Hallname, date);
        }
    }
}

