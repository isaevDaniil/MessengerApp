using MessengerApp.Enums;
using System;

namespace MessengerApp.DAL.Entities
{
    public class ChatEvent : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public DateTime EventTime { get; set; }

        public EventType Type { get; set; }
    }
}
