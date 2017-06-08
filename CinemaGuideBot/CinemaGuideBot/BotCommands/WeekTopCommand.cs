using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class WeekTopCommand: BaseCommand
    {
        public WeekTopCommand() : base("/weektop", "5 самых популярных фильмов недели")
        {
        }

        public override void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter)
        {
            var topMovies = moviesInfoGetter.GetWeekTopMovies();
            botClient.SendTextMessageAsync(request.Chat.Id, string.Join("\r\n", topMovies));
            Logger.Debug("for {0} displayed week top", request.From.ToFormattedString());
        }
    }
}
