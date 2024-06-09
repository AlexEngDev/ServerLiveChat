using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServerLiveChat.Data;
using ServerLiveChat.Helpers;
using ServerLiveChat.Hubs;
using ServerLiveChat.Models;
using System.Globalization;
using System.Xml.Linq;

namespace ServerLiveChat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private static List<User> _user = new List<User>();

        public LiveChatDBContext _liveChatDBContext;
        private readonly ILogger<WeatherForecastController> _logger;

        public UserController(ILogger<WeatherForecastController> logger, LiveChatDBContext liveChatDBContext, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _liveChatDBContext = liveChatDBContext;
            _hubContext = hubContext;
        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> Get()
        {
            return _liveChatDBContext.Users.ToList();
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(User user)
        {
            _liveChatDBContext.Users.Add(user);
            await _liveChatDBContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userSignalRId)
        {
            User user = _liveChatDBContext.Users.FirstOrDefault(u => u.SignalRID == userSignalRId);
            if (user == null)
            {
                return NotFound($"User not found.");
            }

            _liveChatDBContext.Users.Remove(user);
            await _liveChatDBContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("DeleteAllUsers")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            _liveChatDBContext.Users.RemoveRange(_liveChatDBContext.Users);
            await _liveChatDBContext.SaveChangesAsync();
            return Ok();
        }   


        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddUserMessage(string userName, string userMessageText)
        {
            User user = _liveChatDBContext.Users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                return NotFound($"User '{userName}' not found.");
            }

            UserMessage userMessage = new UserMessage
            {
                UserId = user.Id,
                Message = userMessageText
            };  
            _liveChatDBContext.UserMessages.Add(userMessage);
            await _liveChatDBContext.SaveChangesAsync();
            return Ok();
        }


        // Метод для отправки сообщений только тем пользователям, которые находятся в заданном радиусе
        [NonAction]
        public async Task SendMessageToNearbyUsers(int userId, string message)
        {

            // В данном методе реализована отправка сообщений только тем пользователям, которые находятся в заданном радиусе от отправителя

            var userSenderLocation = _liveChatDBContext.Users.FirstOrDefault(u => u.Id == userId);
            string userName = userSenderLocation.Name;

            var allUsersFromBD = _liveChatDBContext.Users.ToList();


            if (userSenderLocation != null)
            {
                foreach (var userLocationFromDB in allUsersFromBD)
                {
                    if  (
                        (double.TryParse(userSenderLocation.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double userSenderLocationlatitude)
                        && double.TryParse(userSenderLocation.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double userSenderLocationlongitude)
                        && double.TryParse(userLocationFromDB.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double userLocationFromDBLatitude)
                        && double.TryParse(userLocationFromDB.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out double userLocationFromDBLongitude))
                        )
                    {
                        double distance = GeoLocationCalculator.CalculateDistance(userSenderLocationlatitude, userSenderLocationlongitude, userLocationFromDBLatitude, userLocationFromDBLongitude);
                        if (distance <= 5) // Радиус 1 км (можете изменить по своему усмотрению)
                        {
                            var findedUserBySignlRId = userLocationFromDB.SignalRID;
                    
                            await _hubContext.Clients.Client(findedUserBySignlRId).SendAsync("UserSendGeoMessage", userName, message);
                        }
                    }
                }
            }   

            //var userLocation = _liveChatDBContext.Users.FirstOrDefault(u => u.Id == userId);
            //string userName = userLocation.Name;

            //var allUsers = _liveChatDBContext.Users.ToList();

            //if (userLocation != null)
            //{
            //    foreach (var location in allUsers)
            //    {
            //        if (location.Id != userId && (double.TryParse(userLocation.Latitude, out double latitude1) && double.TryParse(userLocation.Longitude, out double longitude2) && double.TryParse(location.Latitude, out double longitude3) && double.TryParse(location.Longitude, out double longitude4)))
            //        {
            //            double distance = GeoLocationCalculator.CalculateDistance(latitude1, longitude2, longitude3, longitude4);
            //            if (distance <= 1) // Радиус 1 км (можете изменить по своему усмотрению)
            //            {
            //                await _hubContext.Clients.User(location.Id.ToString()).SendAsync("UserSendGeoMessage", userName, message);
            //            }
            //        }
            //    }
            //}
        }




    }

}
