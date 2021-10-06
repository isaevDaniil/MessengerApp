using System;

namespace MessengerApp.Models
{
    public class MessageModel
    {
        public int Id { get; set; }

        public string SenderName { get; set; }

        public string Text { get; set; }

        public DateTime SendTime { get; set; }
    }
}
