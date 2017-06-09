using System.Linq;
using CinemaGuideBot.Cinema.MoviesInfoGetters;
using CinemaGuideBot.Cinema.MovieInfoFormatters;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public class WeekTopCommand: BaseCommand<string>
    {
        private readonly IMoviesInfoGetter moviesInfoGetter;
        private readonly IMovieInfoFormatter movieInfoFormatter;
        public WeekTopCommand(IMoviesInfoGetter infoGetter, IMovieInfoFormatter movieInfoFormatter) 
            : base("/weektop", "5 самых популярных фильмов недели")
        {
            this.movieInfoFormatter = movieInfoFormatter;
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string request)
        {
            var topMovies = moviesInfoGetter.GetWeekTopMovies().Select(movieInfoFormatter.Format);
            Logger.Debug("displayed week top");
            return string.Join("\r\n", topMovies);
        }
    }
}
