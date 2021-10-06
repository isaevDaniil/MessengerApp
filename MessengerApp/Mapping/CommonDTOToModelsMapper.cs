using MessengerApp.BLL.DTO;
using MessengerApp.Models;

namespace MessengerApp.Mapping
{
    public static class CommonDTOToModelsMapper
    {
        public static MessageModel MapMessage(MessageDTO messageDto)
        {
            return new MessageModel { Id = messageDto.Id, SenderName = messageDto.SenderLogin, SendTime = messageDto.SendTime, Text = messageDto.Text };
        }

        public static ChatEventModel MapChatEvent(ChatEventDTO chatEventDto)
        {
            return new ChatEventModel { Id = chatEventDto.Id, CreatorName = chatEventDto.CreatorName, EventType = chatEventDto.EventType, EventTime = chatEventDto.EventTime };
        }

        public static ChatModel MapChat(ChatDTO chatDto)
        {
            return new ChatModel { Id = chatDto.Id, Name = chatDto.Name, nUnreadMessages = chatDto.nUnreadMessages };
        }
    }
}
