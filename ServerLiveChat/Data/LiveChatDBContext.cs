using Microsoft.EntityFrameworkCore;
using ServerLiveChat.Models;

namespace ServerLiveChat.Data
{
    public class LiveChatDBContext: DbContext
    {
        public LiveChatDBContext(DbContextOptions<LiveChatDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserMessage> UserMessages { get; set; } = null!;
    }   
 
}
