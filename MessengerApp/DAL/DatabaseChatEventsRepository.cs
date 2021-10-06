using MessengerApp.DAL.EF;
using MessengerApp.DAL.Entities;
using MessengerApp.DAL.Intrefaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApp.DAL
{
    public class DatabaseChatEventsRepository : IChatEventsRepository
    {
        private MessengerAppContext _db;

        public DatabaseChatEventsRepository(MessengerAppContext db)
        {
            _db = db;
        }

        public async Task<ChatEvent> AddChatEventAsync(ChatEvent chatEvent)
        {
            var ev = await _db.ChatEvents.AddAsync(chatEvent);
            await _db.SaveChangesAsync();
            return ev.Entity;
        }

        public async Task<List<ChatEvent>> GetAllEventsOfChatAsync(int chatId)
        {
            var chatEvents = await _db.ChatEvents.Where(e => e.ChatId == chatId).ToListAsync();
            return chatEvents;
        }
    }
}
