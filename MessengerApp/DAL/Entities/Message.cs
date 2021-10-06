using System;

namespace MessengerApp.DAL.Entities
{
    public class Message : BaseEntity
    {
        public string Text { get; set; }

        public DateTime SendTime { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
