using MessengerApp.DAL.EF;
using MessengerApp.DAL.Entities;
using MessengerApp.DAL.Intrefaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApp.DAL
{
    public class DatabaseMessagesRepository : IMessagesRepository
    {
        private MessengerAppContext _db;

        public DatabaseMessagesRepository(MessengerAppContext db)
        {
            _db = db;
        }

        public async Task<BotMessage> AddBotMessageAsync(BotMessage message)
        {
            var mess = await _db.BotsMessages.AddAsync(message);
            await _db.SaveChangesAsync();
            return mess.Entity;
        }

        public async Task<UserMessage> AddUserMessageAsync(UserMessage message)
        {
            var mess = await _db.UsersMessages.AddAsync(message);
            await _db.SaveChangesAsync();
            return mess.Entity;
        }

        public async Task<bool> DeleteUserMessageByIdAsync(int id)
        {
            var message = await _db.UsersMessages.FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
                return false;
            _db.UsersMessages.Remove(message);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<BotMessage>> GetAllBotsMessagesOfChatAsync(int chatId)
        {
            var botsMessages = await _db.BotsMessages.Where(m => m.ChatId == chatId).Include(m => m.Bot).ToListAsync();
            return botsMessages;
        }

        public async Task<List<UserMessage>> GetAllUsersMessagesOfChatAsync(int chatId)
        {
            var usersMessages = await _db.UsersMessages.Where(m => m.ChatId == chatId).Include(m => m.User).ToListAsync();
            return usersMessages;
        }
    }
}
