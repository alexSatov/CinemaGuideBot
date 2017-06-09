using System.Linq;
using CinemaGuideBot.Cinema.MoviesInfoGetters;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public class WeekTopCommand: BotCommand<string>
    {
        private readonly IMoviesInfoGetter moviesInfoGetter;

        public WeekTopCommand(IMoviesInfoGetter infoGetter) : base("/weektop")
        {
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string request)
        {
            var topMovies = moviesInfoGetter.GetWeekTopMovies().Select(Bot.BotReply.ReplyFrom);
            Logger.Debug("displayed week top");
            return string.Join("\r\n", topMovies);
        }
    }
}
