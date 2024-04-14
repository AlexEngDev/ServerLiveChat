using Microsoft.AspNetCore.SignalR;
using ServerLiveChat.Models;

namespace ServerLiveChat.Hubs
{
    public class ChatHub : Hub
    {

        public async Task JoinChat(User user)
        {
            await Clients.All.SendAsync("ReceiveMessage", "admin", $"{user.Name} has joined");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
