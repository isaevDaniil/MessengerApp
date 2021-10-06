using MessengerApp.BLL.DTO;
using MessengerApp.BLL.Interfaces;
using MessengerApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MessengerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : Controller
    {
        private IBotService _botService;

        public BotController(IBotService botService)
        {
            _botService = botService;
        }

        [HttpPost("registerBot")]
        public async Task<IActionResult> RegisterBot([FromForm] string botName)
        {
            try
            {
                await _botService.RegisterBotAsync(botName);
                return Ok("Бот " + botName + " зарегистрирован");
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }            
        }

        [HttpPost("acceptResponse")]
        public void AcceptResponseFromBotAPI([FromBody] ResponseFromBot model)
        {
            if (!ModelState.IsValid)
            {
                return;
            }
            _botService.HandleResponseFromBotAPI(new ResponseFromBotDTO { ChatId = model.ChatId, BotAnswer = model.BotAnswer, BotName = model.BotName });
        }
    }
}
