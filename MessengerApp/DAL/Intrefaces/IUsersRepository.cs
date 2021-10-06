using MessengerApp.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.DAL.Intrefaces
{
    public interface IUsersRepository
    {
        public Task<User> CreateUserAsync(User user);

        public Task<List<User>> GetAllUsersAsync();

        public Task<User> GetUserWithChatsByLoginAsync(string login);

        public void SetUserLastVisitTimeNow(ChatsUsers chatUser);
    }
}
