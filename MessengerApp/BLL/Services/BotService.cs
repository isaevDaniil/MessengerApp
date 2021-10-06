using MessengerApp.BLL.DTO;
using MessengerApp.BLL.Interfaces;
using MessengerApp.DAL.Entities;
using MessengerApp.DAL.Intrefaces;
using System;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Services
{
    public class BotService : IBotService
    {
        private IUnitOfWork _repository;

        public BotService(IUnitOfWork unitOfWork)
        {
            _repository = unitOfWork;
        }

        public void HandleResponseFromBotAPI(ResponseFromBotDTO responseFromBotDto)
        {
            var bot = _repository.Bots.GetBotByNameAsync(responseFromBotDto.BotName).Result;
            if (bot == null) 
            {
                return;
            }
            _repository.Messages.AddBotMessageAsync(new BotMessage { BotId = bot.Id, ChatId = responseFromBotDto.ChatId, SendTime = DateTime.Now, Text = responseFromBotDto.BotAnswer }).Wait();
        }

        public async Task RegisterBotAsync(string botName)
        {
            var bots = await _repository.Bots.GetAllBotsAsync();
            foreach (var bot in bots)
            {
                if (bot.Name == botName)
                {
                    throw new ArgumentException("Бот с именем " + botName + " уже зарегистрирован");
                }
            }
            await _repository.Bots.RegisterBotAsync(new Bot { Name = botName });
        }
    }
}
