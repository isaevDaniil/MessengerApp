using MessengerApp.DAL.Intrefaces;

namespace MessengerApp.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        //возможно надо сделать один db и тут создавать экземпляры репозиториев
        public EFUnitOfWork(IUsersRepository usersRepository, IMessagesRepository messagesRepository, IChatsRepository chatsRepository, IChatEventsRepository chatEventsRepository, IBotsRepository botsRepository)
        {
            Users = usersRepository;
            Messages = messagesRepository;
            Chats = chatsRepository;
            ChatEvents = chatEventsRepository;
            Bots = botsRepository;
        }

        public IUsersRepository Users { get; }

        public IMessagesRepository Messages { get; }

        public IChatsRepository Chats { get; }

        public IChatEventsRepository ChatEvents { get; }

        public IBotsRepository Bots { get; }
    }
}
