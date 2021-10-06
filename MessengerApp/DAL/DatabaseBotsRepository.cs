using MessengerApp.DAL.EF;
using MessengerApp.DAL.Entities;
using MessengerApp.DAL.Intrefaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.DAL
{
    public class DatabaseBotsRepository : IBotsRepository
    {
        private MessengerAppContext _db;

        public DatabaseBotsRepository(MessengerAppContext db)
        {
            _db = db;
        }

        public async Task<List<Bot>> GetAllBotsAsync()
        {
            var bots = await _db.Bots.ToListAsync();
            return bots;
        }

        public async Task<Bot> GetBotByNameAsync(string botName)
        {
            var bot = await _db.Bots.FirstOrDefaultAsync(b => b.Name == botName);
            return bot;
        }

        public async Task<Bot> RegisterBotAsync(Bot bot)
        {
            var registeredBot = await _db.Bots.AddAsync(bot);
            await _db.SaveChangesAsync();
            return registeredBot.Entity;
        }
    }
}
