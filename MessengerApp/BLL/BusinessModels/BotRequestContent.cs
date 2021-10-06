namespace MessengerApp.BLL.BusinessModels
{
    public class BotRequestContent
    {
        public string MessageText { get; set; }

        public string BotName { get; set; }

        public int ChatId { get; set; }

        public string BackUrl { get; set; }
    }
}
