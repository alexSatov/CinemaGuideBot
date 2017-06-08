using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MovieInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public interface ICommand
    {
        void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter);
        string HelpText { get; }
        string Name { get; }
    }
}
