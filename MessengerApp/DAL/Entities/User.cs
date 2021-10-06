using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MessengerApp.DAL.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string Login { get; set; }

        public string Password { get; set; }

        public List<Chat> Chats { get; set; } = new List<Chat>();

        public List<ChatsUsers> ChatsUsers { get; set; } = new List<ChatsUsers>();
    }
}
