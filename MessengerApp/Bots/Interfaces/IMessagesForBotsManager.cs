using MessengerApp.BLL.BusinessModels;

namespace MessengerApp.Bots.Interfaces
{
    public interface IMessagesForBotsManager
    {
        public void EnqueueMessageDataAsync(DataForBots model);
    }
}
