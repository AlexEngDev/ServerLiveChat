using System.ComponentModel.DataAnnotations;

namespace ServerLiveChat.Models
{
      public class User
    {
        [Key]
        public int Id { get; set; }
        public string SignalRID { get; set; } = string.Empty;   
        public string Name { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;

        public ICollection<UserMessage>? Messages { get; set; }
    }
}
