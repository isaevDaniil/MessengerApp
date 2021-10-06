using MessengerApp.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.DAL.Intrefaces
{
    public interface IBotsRepository
    {
        public Task<Bot> RegisterBotAsync(Bot bot);

        public Task<List<Bot>> GetAllBotsAsync();

        public Task<Bot> GetBotByNameAsync(string botName);
    }
}
