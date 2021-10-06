using MessengerApp.BLL.DTO;
using MessengerApp.BLL.Interfaces;
using MessengerApp.Mapping;
using MessengerApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("createNew")]
        public async Task<IActionResult> CreateNew([FromBody] ChatCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "Некорректные данные" });
            }
            try
            {
                await _chatService.CreateChatAsync(new ChatCreateDTO { CreatorLogin = User.Identity.Name, ChatName = model.ChatName, ParticipantsLogins = model.ParticipantsLogins });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }          

            return Ok("Чат успешно создан");
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "Некорректные данные" });
            }
            try
            {
                var messageDto = await _chatService.SendMessageAsync(new MessageDTO { SenderLogin = User.Identity.Name, ChatId = model.ChatId, Text = model.Text, SendTime = DateTime.Now });
                return Ok(CommonDTOToModelsMapper.MapMessage(messageDto));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }         
        }

        [HttpGet("getAllMessagesAndEvents/{chatId}")]
        public async Task<IActionResult> GetAllChatMessagesAndEvents(int chatId)
        {
            if (chatId <= 0)
                return BadRequest(new { errorText = "Некорректные данные" });
            try
            {
                var messagesAndEventsDto = await _chatService.GetMessagesAndEventsAsync(User.Identity.Name, chatId);
                var messages = messagesAndEventsDto.MessagesDTO.Select(m => CommonDTOToModelsMapper.MapMessage(m));
                var events = messagesAndEventsDto.EventsDTO.Select(e => CommonDTOToModelsMapper.MapChatEvent(e));
                return Ok(new { messages, events });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }
        }

        [HttpPost("inviteUser")]
        public async Task<IActionResult> InviteUserToChat([FromBody] InviteUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "Некорректные данные" });
            }
            try
            {
                await _chatService.InviteUserToChatAsync(new InviteUserDTO { InvitingUserLogin = User.Identity.Name, InvitedUserLogin = model.InvitedUserLogin, ChatId = model.ChatId });
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }      
        }

        [HttpGet("getAllChats")]
        public async Task<IActionResult> GetAllChats()
        {
            try
            {
                var chatsDto = await _chatService.GetAllUserChatsAsync(User.Identity.Name);
                var chats = chatsDto.Select(ch => CommonDTOToModelsMapper.MapChat(ch));
                return Ok(chatsDto);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }
        }

        [HttpDelete("deleteMessage")]
        public async Task<IActionResult> DeleteMessage([FromBody] DeleteMessageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "Некорректные данные" });
            }
            try
            {
                await _chatService.DeleteMessageAsync(new DeleteMessageDTO { UserLogin = User.Identity.Name, ChatId = model.ChatId, MessageId = model.MessageId });
                return Ok("Сообщение удалено");
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }   
        }

        [HttpPost("inviteBot")]
        public async Task<IActionResult> InviteBot([FromBody] InviteBotModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "Некорректные данные" });
            }
            try
            {
                await _chatService.InviteBotToChatAsync(new InviteBotDTO { UserLogin = User.Identity.Name, ChatId = model.ChatId, AddedBotName = model.AddedBotName });
                return Ok("Бот доваблен в чат");
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorText = e.Message });
            }
        }
    }
}
