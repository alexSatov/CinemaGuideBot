using Telegram.Bot;
using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public interface ICommandExecutor
    {
        ICommand[] GetAviableCommands();
        void Register(ICommand command);
        void Execute(Bot bot, Message message);
    }
}
