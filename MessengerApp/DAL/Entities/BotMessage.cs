namespace MessengerApp.DAL.Entities
{
    public class BotMessage : Message
    {
        public int BotId { get; set; }
        public Bot Bot { get; set; }
    }
}
