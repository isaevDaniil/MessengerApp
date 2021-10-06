using System.Collections.Generic;

namespace MessengerApp.DAL.Entities
{
    public class Chat : BaseEntity
    {
        public string Name { get; set; }

        public int CreatorId { get; set; }

        public List<User> Users { get; set; }

        public List<UserMessage> UsersMessages { get; set; } = new List<UserMessage>();

        public List<BotMessage> BotsMessages { get; set; } = new List<BotMessage>();

        public List<ChatEvent> ChatEvents { get; set; } = new List<ChatEvent>();

        public List<Bot> Bots { get; set; } = new List<Bot>();

        public List<ChatsUsers> ChatsUsers { get; set; } = new List<ChatsUsers>();
    }
}
