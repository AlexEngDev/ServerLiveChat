
namespace ServerLiveChat.Models
{
    public class UserMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;

        public User User { get; set; } = null!;

    }
}
