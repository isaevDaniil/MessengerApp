using MessengerApp.BLL.BusinessModels;
using MessengerApp.Bots.Interfaces;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MessengerApp.Bots.Services
{
    public class BackgroundMessagesDataQueue : IBackgroundMessagesDataQueue
    {
        private readonly Channel<DataForBots> _messageQueue;

        public BackgroundMessagesDataQueue(int capacity)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _messageQueue = Channel.CreateBounded<DataForBots>(options);
        }

        public async ValueTask<DataForBots> DequeueMessageDataAsync(CancellationToken cancellationToken)
        {
            var message = await _messageQueue.Reader.ReadAsync(cancellationToken);
            return message;
        }

        public async ValueTask EnqueueMessageDataAsync(DataForBots messageData)
        {
            if (messageData == null)
            {
                throw new ArgumentNullException(nameof(messageData));
            }
            await _messageQueue.Writer.WriteAsync(messageData);
        }
    }
}
