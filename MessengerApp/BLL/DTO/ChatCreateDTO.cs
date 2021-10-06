namespace MessengerApp.BLL.DTO
{
    public class ChatCreateDTO
    {
        public string CreatorLogin { get; set; }

        public string ChatName { get; set; }

        public string[] ParticipantsLogins { get; set; }
    }
}
