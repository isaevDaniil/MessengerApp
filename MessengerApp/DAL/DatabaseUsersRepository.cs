using MessengerApp.DAL.EF;
using MessengerApp.DAL.Entities;
using MessengerApp.DAL.Intrefaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.DAL
{
    public class DatabaseUsersRepository : IUsersRepository
    {
        private MessengerAppContext _db;

        public DatabaseUsersRepository(MessengerAppContext db)
        {
            _db = db;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var us = await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return us.Entity;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _db.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUserWithChatsByLoginAsync(string login)
        {
            var user = await _db.Users.Include(u => u.Chats).FirstOrDefaultAsync(u => u.Login == login);
            return user;
        }

        public void SetUserLastVisitTimeNow(ChatsUsers chatUser)
        {
            chatUser.LastVisitTime = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
