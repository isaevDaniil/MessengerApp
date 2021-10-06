using MessengerApp.BLL.BusinessModels;
using MessengerApp.Bots.Interfaces;
using MessengerApp.DAL.Intrefaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerApp.Bots.Services
{
    public class QueuedHostedService : BackgroundService
    {
        private IServiceProvider _services;
        private SemaphoreSlim _semaphoreSlim;
        private string _pathToBotsAPI;
        private string _backUrlToBots;

        public IBackgroundMessagesDataQueue MessagesQueue { get; }

        public QueuedHostedService(IBackgroundMessagesDataQueue messagesQueue, IServiceProvider serviceProvider, int maxBotsThreadsCount, string pathToBotsAPI, string backUrlToBots)
        {
            MessagesQueue = messagesQueue;
            _services = serviceProvider;
            _semaphoreSlim = new SemaphoreSlim(maxBotsThreadsCount);
            _pathToBotsAPI = pathToBotsAPI;
            _backUrlToBots = backUrlToBots;
        }     

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var messageData = await MessagesQueue.DequeueMessageDataAsync(stoppingToken);
                await _semaphoreSlim.WaitAsync();              
                var t = Task.Run(() => NotifyBots(messageData, stoppingToken));
            }
        }

        private void NotifyBots(DataForBots model, CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                var chatsRep = scope.ServiceProvider.GetRequiredService<IChatsRepository>();
                var chatTask = chatsRep.GetChatWithBotsAsync(model.ChatId);
                var bots = chatTask.Result.Bots;
                var messRep = scope.ServiceProvider.GetRequiredService<IMessagesRepository>();
                using (var scopeForBot = _services.CreateScope())
                {
                    foreach (var bot in bots)
                    {
                        SendRequest(new BotRequestContent { MessageText = model.MessageText, BotName = bot.Name, ChatId = model.ChatId, BackUrl = _backUrlToBots });
                    }
                }
            }
            _semaphoreSlim.Release();
        }

        private void SendRequest(BotRequestContent botRequestContent)
        {
            using (var httpClient = new HttpClient())
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(botRequestContent));
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                try
                {
                    httpClient.PostAsync(_pathToBotsAPI, httpContent).Wait();               
                }
                catch (Exception)
                {
                }             
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
