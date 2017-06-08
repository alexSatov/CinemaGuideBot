using CinemaGuideBot.Domain.MoviesInfoGetter;
using NLog;
using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public class WeekTopCommand: ICommand
    {
        private static readonly Logger logger = LogManager.GetLogger("WeekTopCommand");
        public void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter)
        {
            var topMovies = moviesInfoGetter.GetWeekTopMovies();
            botClient.SendTextMessageAsync(request.Chat.Id, string.Join("\r\n", topMovies));
            logger.Debug("for {0} displayed week top", request.From.ToFormattedString());
        }

        public string HelpText => "show week top 5 movies";
        public string Name => "/top_week";
    }
}
