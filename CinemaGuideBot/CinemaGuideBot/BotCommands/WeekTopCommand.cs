using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class WeekTopCommand: BaseCommand
    {
        private readonly IMoviesInfoGetter moviesInfoGetter;

        public WeekTopCommand(IMoviesInfoGetter infoGetter) : base("/weektop", "5 самых популярных фильмов недели")
        {
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string request)
        {
            var topMovies = moviesInfoGetter.GetWeekTopMovies();
            Logger.Debug("displayed week top");
            return string.Join("\r\n", topMovies);
        }
    }
}
