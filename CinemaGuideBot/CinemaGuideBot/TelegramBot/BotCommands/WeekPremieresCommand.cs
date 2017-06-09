using System.Linq;
using CinemaGuideBot.Cinema.MoviesInfoGetters;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public class WeekPremieresCommand : BotCommand<string>
    {
        private readonly IMoviesInfoGetter moviesInfoGetter;

        public WeekPremieresCommand(IMoviesInfoGetter infoGetter) : base("/weeknew")
        {
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string request)
        {
            var newMovies = moviesInfoGetter.GetWeekNewMovies().Select(Bot.BotReply.ReplyFrom);
            Logger.Debug("displayed week top");
            return string.Join("\r\n", newMovies);
        }
    }
}
