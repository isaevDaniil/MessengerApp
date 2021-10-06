using System;

namespace MessengerApp.BLL.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }

        public string SenderLogin { get; set; }

        public string Text { get; set; }

        public int ChatId { get; set; }

        public DateTime SendTime { get; set; }
    }
}
