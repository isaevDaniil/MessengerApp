using MessengerApp.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.DAL.Intrefaces
{
    public interface IChatsRepository
    {
        public Task<Chat> CreateChatAsync(Chat chat);

        public Task<bool> AddUserToChatAsync(Chat chat, User user);

        public Task<bool> AddBotToChatAsync(Chat chat, Bot bot);

        public Task<List<Chat>> GetUserChatsWithMessagesAsync(int userId);

        public Task<Chat> GetChatWithBotsAsync(int chatId);
    }
}
