using Microsoft.AspNetCore.SignalR;
using ServerLiveChat.Models;

namespace ServerLiveChat.Hubs
{
    public class ChatHub : Hub
    {

        public async Task JoinChat(string user)
        {
            await Clients.All.SendAsync("UserJoin", user);
        }

        public async Task SendMessage(string user, string message, string timestamp, string latitude, string longitude)
        {
            DateTime dateTime = DateTime.Parse(timestamp);
            string formattedTimestamp = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            Console.WriteLine($"User: {user} Message: {message} Timestamp: {formattedTimestamp} Latitude: {latitude} Longitude: {longitude}");

            await Clients.All.SendAsync("ReceiveMessage", user, message, formattedTimestamp);
        }



    }
}
