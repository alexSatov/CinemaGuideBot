using NLog;
using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    class NewWeekCommand : ICommand
    {
        private static readonly Logger logger = LogManager.GetLogger("NewWeekCommand");
        public void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter)
        {
            var newMovies = moviesInfoGetter.GetWeekNewMovies();
            botClient.SendTextMessageAsync(request.Chat.Id, string.Join("\r\n", newMovies));
            logger.Debug("for {0} displayed week top", request.From.ToFormattedString());
        }

        public string HelpText => "show new movies of week";
        public string Name => "/new_week";
    }
}
