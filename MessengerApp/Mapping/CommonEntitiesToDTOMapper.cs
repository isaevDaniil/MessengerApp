using MessengerApp.BLL.DTO;
using MessengerApp.DAL.Entities;

namespace MessengerApp.Mapping
{
    public static class CommonEntitiesToDTOMapper
    {
        public static MessageDTO MapUserMessage(UserMessage userMessage)
        {
            return new MessageDTO { Id = userMessage.Id, SenderLogin = userMessage.User.Login, ChatId = userMessage.ChatId, Text = userMessage.Text, SendTime = userMessage.SendTime };
        }

        public static MessageDTO MapBotMessage(BotMessage botMessage)
        {
            return new MessageDTO { Id = botMessage.Id, SenderLogin = botMessage.Bot.Name, ChatId = botMessage.ChatId, Text = botMessage.Text, SendTime = botMessage.SendTime };
        }

        public static ChatEventDTO MapChatEvent(ChatEvent chatEvent)
        {
            return new ChatEventDTO { Id = chatEvent.Id, CreatorName = chatEvent.User.Login, EventType = chatEvent.Type.ToString(), EventTime = chatEvent.EventTime };
        }
    }
}
