using MessengerApp.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.DAL.Intrefaces
{
    public interface IChatEventsRepository
    {
        public Task<ChatEvent> AddChatEventAsync(ChatEvent chatEvent);

        public Task<List<ChatEvent>> GetAllEventsOfChatAsync(int chatId);
    }
}
