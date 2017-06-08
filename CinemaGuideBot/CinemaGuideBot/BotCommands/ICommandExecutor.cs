using CinemaGuideBot.Domain.MoviesInfoGetter;
using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public interface ICommandExecutor<T>
    {
        IMoviesInfoGetter MoviesInfoGetter { get; set; }
        ICommand<T>[] GetAviableCommands();
        void Register(ICommand<T> command);
        void Execute(Bot bot, Message message);
    }
}
