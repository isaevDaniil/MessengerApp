using MessengerApp.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.DAL.Intrefaces
{
    public interface IMessagesRepository
    {
        public Task<UserMessage> AddUserMessageAsync(UserMessage message);

        public Task<List<UserMessage>> GetAllUsersMessagesOfChatAsync(int chatId);

        public Task<List<BotMessage>> GetAllBotsMessagesOfChatAsync(int chatId);

        public Task<BotMessage> AddBotMessageAsync(BotMessage message);

        public Task<bool> DeleteUserMessageByIdAsync(int id);
    }
}
