using Microsoft.AspNetCore.SignalR;
using ServerLiveChat.Controllers;
using ServerLiveChat.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServerLiveChat.Data;

namespace ServerLiveChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;
        public LiveChatDBContext _liveChatDBContext;

        public ChatHub(IServiceProvider serviceProvider, LiveChatDBContext liveChatDBContext)
        {
            _serviceProvider = serviceProvider;
            _liveChatDBContext = liveChatDBContext;
        }

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"New connection established with ID: {connectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"Connection with ID: {connectionId} disconnected");
            using (var scope = _serviceProvider.CreateScope())
            {
                var userController = scope.ServiceProvider.GetRequiredService<UserController>();
                await userController.DeleteUser(connectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }


        public async Task JoinChat(User user) 
        {
           
            user.SignalRID = Context.ConnectionId;
            Console.WriteLine($"User {user.Name} joined the chat with connection ID: {user.SignalRID}");

            using (var scope = _serviceProvider.CreateScope())
            {
                var userController = scope.ServiceProvider.GetRequiredService<UserController>();
                await userController.AddUser(user);
                await Clients.All.SendAsync("UserJoin", user.Name);
            }
        }

        public async Task SendMessage(string user, string message, string timestamp, string latitude, string longitude) //da sostituire con SendMessageToNearbyUsersAsync
        {
            DateTime dateTime = DateTime.Parse(timestamp);
            string formattedTimestamp = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            Console.WriteLine($"User: {user} Message: {message} Timestamp: {formattedTimestamp} Latitude: {latitude} Longitude: {longitude}");
            await Clients.All.SendAsync("ReceiveMessage", user, message, formattedTimestamp);
        }

        public async Task SendMessageToNearbyUsersAsync(string userName, string message)  //da usare come SendMessage
        {
            User user = _liveChatDBContext.Users.FirstOrDefault(u => u.Name == userName);

              if (user == null)
                    {
                        return;
                    }

            using (var scope = _serviceProvider.CreateScope())
            {
                var userController = scope.ServiceProvider.GetRequiredService<UserController>();

                await userController.SendMessageToNearbyUsers(user.Id, message);
            }
        }
    }
}
