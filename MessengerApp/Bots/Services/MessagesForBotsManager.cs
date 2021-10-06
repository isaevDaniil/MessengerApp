using MessengerApp.BLL.BusinessModels;
using MessengerApp.Bots.Interfaces;

namespace MessengerApp.Bots.Services
{
    public class MessagesForBotsManager : IMessagesForBotsManager
    {
        private readonly IBackgroundMessagesDataQueue _taskQueue;

        public MessagesForBotsManager(IBackgroundMessagesDataQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }

        public async void EnqueueMessageDataAsync(DataForBots model)
        {
            await _taskQueue.EnqueueMessageDataAsync(model);
        }
    }
}
