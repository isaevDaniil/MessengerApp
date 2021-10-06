using MessengerApp.BLL.BusinessModels;
using MessengerApp.BLL.DTO;
using MessengerApp.BLL.Interfaces;
using MessengerApp.Bots.Interfaces;
using MessengerApp.DAL.Entities;
using MessengerApp.DAL.Intrefaces;
using MessengerApp.Enums;
using MessengerApp.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Services
{
    public class ChatService : IChatService
    {
        private IUnitOfWork _repository;
        private IMessagesForBotsManager _messagesForBotsManager;

        public ChatService(IUnitOfWork unitOfWork, IMessagesForBotsManager messagesForBotsManager)
        {
            _repository = unitOfWork;
            _messagesForBotsManager = messagesForBotsManager;
        }

        public async Task CreateChatAsync(ChatCreateDTO chatDto)
        {
            var creator = await _repository.Users.GetUserWithChatsByLoginAsync(chatDto.CreatorLogin);
            var participants = new List<User>();
            foreach (var userLogin in chatDto.ParticipantsLogins)
            {
                var participant = await _repository.Users.GetUserWithChatsByLoginAsync(userLogin);
                if (participant == null)
                {
                    throw new ArgumentException("Пользователь " + userLogin + " не найден");
                }
                participants.Add(participant);
            }
            participants.Add(creator);
            await _repository.Chats.CreateChatAsync(new Chat { Name = chatDto.ChatName, CreatorId = creator.Id, Users = participants });
        }

        public async Task DeleteMessageAsync(DeleteMessageDTO deleteMessageDto)
        {
            var user = await _repository.Users.GetUserWithChatsByLoginAsync(deleteMessageDto.UserLogin);
            var chat = user.Chats.FirstOrDefault(ch => ch.Id == deleteMessageDto.ChatId);
            if (chat == null)
            {
                throw new ArgumentException("Пользователь " + user.Login + " не имеет такого чата");
            }
            var chatMessages = await _repository.Messages.GetAllUsersMessagesOfChatAsync(chat.Id);
            var userMessages = chatMessages.Where(m => m.UserId == user.Id);
            if (userMessages.Count() == 0)
            {
                throw new ArgumentException("У пользователя " + user.Login + " нет сообщений");
            }
            var messageToDelete = userMessages.FirstOrDefault(m => m.Id == deleteMessageDto.MessageId);
            if (messageToDelete == null)
            {
                throw new ArgumentException("Сообщение не найдено");
            }
            var isDeleted = await _repository.Messages.DeleteUserMessageByIdAsync(deleteMessageDto.MessageId);
            if (!isDeleted)
                throw new ArgumentException("Произошла ошибка на уровне базы данных");
        }

        public async Task<List<ChatDTO>> GetAllUserChatsAsync(string userLogin)
        {
            var currentUser = await _repository.Users.GetUserWithChatsByLoginAsync(userLogin);
            if (currentUser == null)
                throw new ArgumentException("Пользователь " + userLogin + " не найден");

            var userChats = await _repository.Chats.GetUserChatsWithMessagesAsync(currentUser.Id);
            if (userChats.Count == 0)
            {
                throw new ArgumentException("У пользователя " + userLogin + " нет чатов");
            }

            var chatsInfo = userChats.Join(currentUser.ChatsUsers,
                ch => ch.Id, chUs => chUs.ChatId, (ch, chUs) => new { Id = ch.Id, Name = ch.Name, LastVisitTime = chUs.LastVisitTime, botMess = ch.BotsMessages, userMess = ch.UsersMessages, singInTime = chUs.SignInTime });

            var resultChatDTOs = new List<ChatDTO>();
            foreach (var chatInfo in chatsInfo)
            {
                var nUnreadMess = chatInfo.botMess.Where(m => m.SendTime > chatInfo.LastVisitTime && m.SendTime > chatInfo.singInTime).Count() + chatInfo.userMess.Where(m => m.SendTime > chatInfo.LastVisitTime && m.SendTime > chatInfo.singInTime).Count();
                resultChatDTOs.Add(new ChatDTO { Id = chatInfo.Id, Name = chatInfo.Name, nUnreadMessages = nUnreadMess });
            }
            return resultChatDTOs;
        }

        public async Task<MessagesAndEventsDTO> GetMessagesAndEventsAsync(string userLogin, int chatId)
        {
            var user = await _repository.Users.GetUserWithChatsByLoginAsync(userLogin);
            var chatUserInfo = user.ChatsUsers.FirstOrDefault(ch => ch.ChatId == chatId);
            if (chatUserInfo == null)
            {
                throw new ArgumentException("Пользователь " + user.Login + " не имеет такого чата");
            }
            var allUsersMessages = await _repository.Messages.GetAllUsersMessagesOfChatAsync(chatId);
            var allBotsMessages = await _repository.Messages.GetAllBotsMessagesOfChatAsync(chatId);
            var allEvents = await _repository.ChatEvents.GetAllEventsOfChatAsync(chatId);
            var messagesToUser = new List<MessageDTO>();
            var eventToUser = new List<ChatEventDTO>();

            if (chatUserInfo.SignOutTime != default(DateTime))
            {
                messagesToUser = allUsersMessages.Where(m => m.SendTime > chatUserInfo.SignInTime && m.SendTime < chatUserInfo.SignOutTime).Select(m => CommonEntitiesToDTOMapper.MapUserMessage(m)).ToList();
                messagesToUser.AddRange(allBotsMessages.Where(m => m.SendTime > chatUserInfo.SignInTime && m.SendTime < chatUserInfo.SignOutTime).Select(m => CommonEntitiesToDTOMapper.MapBotMessage(m)).ToList());
                var sortedMessagesToUser = messagesToUser.OrderBy(m => m.SendTime);
                eventToUser = allEvents.Where(e => e.EventTime > chatUserInfo.SignInTime && e.EventTime < chatUserInfo.SignOutTime).Select(e => CommonEntitiesToDTOMapper.MapChatEvent(e)).ToList();
            }
            else
            {
                messagesToUser = allUsersMessages.Where(m => m.SendTime > chatUserInfo.SignInTime).Select(m => CommonEntitiesToDTOMapper.MapUserMessage(m)).ToList();
                messagesToUser.AddRange(allBotsMessages.Where(m => m.SendTime > chatUserInfo.SignInTime).Select(m => CommonEntitiesToDTOMapper.MapBotMessage(m)).ToList());
                var sortedMessagesToUser = messagesToUser.OrderBy(m => m.SendTime);
                eventToUser = allEvents.Where(e => e.EventTime > chatUserInfo.SignInTime).Select(e => CommonEntitiesToDTOMapper.MapChatEvent(e)).ToList();
            }
            _repository.Users.SetUserLastVisitTimeNow(chatUserInfo);
            return new MessagesAndEventsDTO { MessagesDTO = messagesToUser, EventsDTO = eventToUser };
        }

        public async Task InviteBotToChatAsync(InviteBotDTO inviteBotDto)
        {
            var user = await _repository.Users.GetUserWithChatsByLoginAsync(inviteBotDto.UserLogin);
            var currentChat = user.Chats.FirstOrDefault(ch => ch.Id == inviteBotDto.ChatId);
            if (currentChat == null)
            {
                throw new ArgumentException("Пользователь " + user.Login + " не имеет такого чата");
            }
            var bot = await _repository.Bots.GetBotByNameAsync(inviteBotDto.AddedBotName);
            if (bot == null)
            {
                throw new ArgumentException("Бот с именем " + inviteBotDto.AddedBotName + " не найден");
            }
            var chatWithBots = await _repository.Chats.GetChatWithBotsAsync(inviteBotDto.ChatId);
            if (chatWithBots.Bots.Contains(bot))
            {
                throw new ArgumentException("Бот с именем " + inviteBotDto.AddedBotName + " уже добавлен в чат");
            }
            if (!(await _repository.Chats.AddBotToChatAsync(currentChat, bot)))
            {
                throw new ArgumentException("Произошла ошибка при обращении к базе данных");
            }
        }

        public async Task InviteUserToChatAsync(InviteUserDTO inviteUserDto)
        {
            var invitingUser = await _repository.Users.GetUserWithChatsByLoginAsync(inviteUserDto.InvitingUserLogin);
            var currentChat = invitingUser.Chats.FirstOrDefault(ch => ch.Id == inviteUserDto.ChatId);
            if (currentChat == null)
            {
                throw new ArgumentException("Пользователь " + invitingUser.Login + " не имеет такого чата");
            }
            var invitedUser = await _repository.Users.GetUserWithChatsByLoginAsync(inviteUserDto.InvitedUserLogin);
            if (invitedUser == null)
                throw new ArgumentException("Пользователь " + inviteUserDto.InvitedUserLogin + " не найден");

            if (invitedUser.Chats.FirstOrDefault(ch => ch.Id == currentChat.Id) != null)
                throw new ArgumentException("Пользователь " + inviteUserDto.InvitedUserLogin + " уже добавлен в чат");

            if (!(await _repository.Chats.AddUserToChatAsync(currentChat, invitedUser)))
            {
                throw new ArgumentException("Произошла ошибка при обращении к базе данных");
            }
            var chatEvent = await _repository.ChatEvents.AddChatEventAsync(new ChatEvent { Type = EventType.UserJoinedChat, EventTime = DateTime.Now, Chat = currentChat, User = invitedUser });
        }

        public async Task<MessageDTO> SendMessageAsync(MessageDTO messageDto)
        {
            var user = await _repository.Users.GetUserWithChatsByLoginAsync(messageDto.SenderLogin);
            var chat = user.Chats.FirstOrDefault(ch => ch.Id == messageDto.ChatId);
            if (chat == null)
            {
                throw new ArgumentException("Пользователь " + user.Login + " не имеет такого чата");
            }
            var message = await _repository.Messages.AddUserMessageAsync(new UserMessage { Text = messageDto.Text, SendTime = DateTime.Now, UserId = user.Id, Chat = chat });
            _repository.Users.SetUserLastVisitTimeNow(user.ChatsUsers.FirstOrDefault(ch => ch.ChatId == chat.Id));

            _messagesForBotsManager.EnqueueMessageDataAsync(new DataForBots { ChatId = chat.Id, MessageText = messageDto.Text });
            return CommonEntitiesToDTOMapper.MapUserMessage(message);
        }
    }
}
