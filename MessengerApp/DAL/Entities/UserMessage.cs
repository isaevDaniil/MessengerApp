namespace MessengerApp.DAL.Entities
{
    public class UserMessage : Message
    {
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
