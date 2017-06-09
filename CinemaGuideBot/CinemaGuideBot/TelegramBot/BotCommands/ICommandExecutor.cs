using Telegram.Bot.Types;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public interface ICommandExecutor<T>
    {
        ICommand<T>[] GetAviableCommands();
        void Register(params ICommand<T>[] newCommands);
        string Execute(Message message);
    }
}
