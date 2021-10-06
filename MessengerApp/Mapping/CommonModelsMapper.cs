using MessengerApp.DAL.Entities;
using MessengerApp.Models;

namespace MessengerApp.Mapping
{
    public static class CommonModelsMapper
    {
        public static MessageModel UserMessageToMessageModel(UserMessage userMessage)
        {
            return new MessageModel { Id = userMessage.Id, SenderName = userMessage.User.Login, SendTime = userMessage.SendTime, Text = userMessage.Text };
        }

        public static MessageModel BotMessageToMessageModel(BotMessage botMessage)
        {
            return new MessageModel { Id = botMessage.Id, SenderName = botMessage.Bot.Name, SendTime = botMessage.SendTime, Text = botMessage.Text };
        }

        public static ChatEventModel ChatEventToChatEventModel(ChatEvent chatEvent)
        {
            return new ChatEventModel { Id = chatEvent.Id, CreatorName = chatEvent.User.Login, EventTime = chatEvent.EventTime, EventType = chatEvent.Type.ToString() };
        }
    }
}
