namespace MessengerApp.BLL.DTO
{
    public class DeleteMessageDTO
    {
        public string UserLogin { get; set; }

        public int ChatId { get; set; }

        public int MessageId { get; set; }
    }
}
