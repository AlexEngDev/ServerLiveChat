using System.ComponentModel.DataAnnotations;

namespace ServerLiveChat.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty; 
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
    }
}
