using Microsoft.AspNetCore.SignalR;

namespace EFCore_Inheritance_Demo_Main9.Hubs
{
    public class TodoHub : Hub 
    {
        // Denne metode sender en meddelelse til alle klienter
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
