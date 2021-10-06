using MessengerApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.DAL.EF
{
    public class MessengerAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<UserMessage> UsersMessages { get; set; }

        public DbSet<BotMessage> BotsMessages { get; set; }

        public DbSet<Bot> Bots { get; set; }

        public DbSet<ChatEvent> ChatEvents { get; set; }

        public MessengerAppContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Chat>()
                .HasMany(ch => ch.Users)
                .WithMany(u => u.Chats)
                .UsingEntity<ChatsUsers>(
                cu => cu
                .HasOne(chatUser => chatUser.User)
                .WithMany(u2 => u2.ChatsUsers)
                .HasForeignKey(chatUser => chatUser.UserId),
                cu => cu
                .HasOne(chatUser => chatUser.Chat)
                .WithMany(ch2 => ch2.ChatsUsers)
                .HasForeignKey(chatUser => chatUser.ChatId),
                cu =>
                {
                    cu.Property(p => p.SignInTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    cu.HasKey(k => new { k.ChatId, k.UserId });
                });
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<Bot>().HasIndex(b => b.Name).IsUnique();
        }
    }
}
