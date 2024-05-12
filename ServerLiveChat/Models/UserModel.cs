using System.ComponentModel.DataAnnotations;

namespace ServerLiveChat.Models
{
      public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<UserMessage>? Messages { get; set; }
    }
}
