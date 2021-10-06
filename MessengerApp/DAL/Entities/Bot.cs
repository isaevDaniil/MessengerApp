using System.Collections.Generic;

namespace MessengerApp.DAL.Entities
{
    public class Bot : BaseEntity
    {
        public string Name { get; set; }

        public List<Chat> Chats { get; set; } = new List<Chat>();
    }
}
