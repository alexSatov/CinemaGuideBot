using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public interface ICommandExecutor
    {
        ICommand[] GetAviableCommands();
        void Register(params ICommand[] newCommands);
        void Execute(Bot bot, Message message);
    }
}
