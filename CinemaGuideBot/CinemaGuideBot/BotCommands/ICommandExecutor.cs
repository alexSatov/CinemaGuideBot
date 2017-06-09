using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public interface ICommandExecutor<T>
    {
        ICommand<T>[] GetAviableCommands();
        void Register(params ICommand<T>[] newCommands);
        void Execute(Bot bot, Message message);
    }
}
