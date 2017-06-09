using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class WeekPremieresCommand : BaseCommand
    {
        private readonly IMoviesInfoGetter moviesInfoGetter;

        public WeekPremieresCommand(IMoviesInfoGetter infoGetter) : base("/weeknew", "премьеры недели")
        {
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string request)
        {
            var newMovies = moviesInfoGetter.GetWeekNewMovies();
            Logger.Debug("displayed week top");
            return string.Join("\r\n", newMovies);
        }
    }
}
