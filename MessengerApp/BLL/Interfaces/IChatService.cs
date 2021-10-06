using MessengerApp.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Interfaces
{
    public interface IChatService
    {
        public Task CreateChatAsync(ChatCreateDTO chatDto);

        public Task<MessageDTO> SendMessageAsync(MessageDTO messageDto);

        public Task<MessagesAndEventsDTO> GetMessagesAndEventsAsync(string userLogin, int chatId);

        public Task InviteUserToChatAsync(InviteUserDTO inviteUserDto);

        public Task<List<ChatDTO>> GetAllUserChatsAsync(string userLogin);

        public Task DeleteMessageAsync(DeleteMessageDTO deleteMessageDto);

        public Task InviteBotToChatAsync(InviteBotDTO inviteBotDto);
    }
}
