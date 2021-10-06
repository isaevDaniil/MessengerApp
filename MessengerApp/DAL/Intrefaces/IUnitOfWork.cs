namespace MessengerApp.DAL.Intrefaces
{
    public interface IUnitOfWork
    {
        public IUsersRepository Users { get; }

        public IMessagesRepository Messages { get; }

        public IChatsRepository Chats { get; }

        public IChatEventsRepository ChatEvents { get; }

        public IBotsRepository Bots { get; }
    }
}
