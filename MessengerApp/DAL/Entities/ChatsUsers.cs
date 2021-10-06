using System;

namespace MessengerApp.DAL.Entities
{
    public class ChatsUsers
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }

        public DateTime SignInTime { get; set; }

        public DateTime SignOutTime { get; set; }

        public DateTime LastVisitTime { get; set; }
    }
}
