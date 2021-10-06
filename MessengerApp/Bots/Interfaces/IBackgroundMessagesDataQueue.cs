using MessengerApp.BLL.BusinessModels;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerApp.Bots.Interfaces
{
    public interface IBackgroundMessagesDataQueue
    {
        ValueTask EnqueueMessageDataAsync(DataForBots messageData);

        ValueTask<DataForBots> DequeueMessageDataAsync(CancellationToken cancellationToken);
    }
}
