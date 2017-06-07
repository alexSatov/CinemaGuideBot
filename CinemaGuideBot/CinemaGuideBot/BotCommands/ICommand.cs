using CinemaGuideBot.Domain;
using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public interface ICommand
    {
        void Execute(Bot botClient, Message request, IMovieInfoGetter movieInfoGetter);
        string HelpText { get; }
        string Name { get; }
    }
}
