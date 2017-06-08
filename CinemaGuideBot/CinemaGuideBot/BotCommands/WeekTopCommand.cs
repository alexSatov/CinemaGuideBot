using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class WeekTopCommand: BaseCommand
    {
        public WeekTopCommand() : base("/top_week", "show week top 5 movies", "WeekTopCommand")
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
