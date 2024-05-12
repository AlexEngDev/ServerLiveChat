using Microsoft.AspNetCore.Mvc;
using ServerLiveChat.Data;
using ServerLiveChat.Models;
using System.Xml.Linq;

namespace ServerLiveChat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        public LiveChatDBContext _liveChatDBContext;
        private readonly ILogger<WeatherForecastController> _logger;

        public UserController(ILogger<WeatherForecastController> logger, LiveChatDBContext liveChatDBContext)
        {
            _logger = logger;
            _liveChatDBContext = liveChatDBContext;
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

    }

}
