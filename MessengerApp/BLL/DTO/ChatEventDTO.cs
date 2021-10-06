using System;

namespace MessengerApp.BLL.DTO
{
    public class ChatEventDTO
    {
        public int Id { get; set; }

        public string CreatorName { get; set; }

        public DateTime EventTime { get; set; }

        public string EventType { get; set; }
    }
}
