using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MovieInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public interface ICommand
    {
        void Execute(Bot botClient, Message request, IMovieInfoGetter movieInfoGetter);
        string HelpText { get; }
        string Name { get; }
    }
}
