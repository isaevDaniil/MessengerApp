using System.Collections.Generic;

namespace MessengerApp.BLL.DTO
{
    public class MessagesAndEventsDTO
    {
        public List<MessageDTO> MessagesDTO { get; set; }

        public List<ChatEventDTO> EventsDTO { get; set; }
    }
}
