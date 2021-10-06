using MessengerApp.DAL.EF;
using MessengerApp.DAL.Entities;
using MessengerApp.DAL.Intrefaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.DAL
{
    public class DatabaseChatsRepository : IChatsRepository
    {
        private MessengerAppContext _db;

        public DatabaseChatsRepository(MessengerAppContext db)
        {
            _db = db;
        }

        public async Task<bool> AddBotToChatAsync(Chat chat, Bot bot)
        {
            chat.Bots.Add(bot);
            var nEditedRows = await _db.SaveChangesAsync();
            if (nEditedRows == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddUserToChatAsync(Chat chat, User user)
        {
            chat.ChatsUsers.Add(new ChatsUsers { User = user, SignInTime = DateTime.Now });
            var nEditedEntries = await _db.SaveChangesAsync();
            if (nEditedEntries == 0) 
            {
                return false;
            }
            return true;
        }

        public async Task<Chat> CreateChatAsync(Chat chat)
        {
            var addedChat = await _db.Chats.AddAsync(chat);
            await _db.SaveChangesAsync();
            return addedChat.Entity;
        }

        public async Task<Chat> GetChatWithBotsAsync(int chatId)
        {
            var chat = await _db.Chats.Include(ch=>ch.Bots).FirstOrDefaultAsync(ch => ch.Id == chatId);
            return chat;
        }

        public async Task<List<Chat>> GetUserChatsWithMessagesAsync(int userId)
        {
            var chats = await _db.Chats.Include(ch => ch.UsersMessages).Include(ch => ch.BotsMessages).ToListAsync();
            return chats;
        }
    }
}
