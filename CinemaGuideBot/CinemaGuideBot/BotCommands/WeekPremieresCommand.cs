using System.Linq;
using CinemaGuideBot.Domain;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class WeekPremieresCommand : BaseCommand<string>
    {
        private readonly IMoviesInfoGetter moviesInfoGetter;
        private readonly IMovieInfoFormatter movieInfoFormatter;
        public WeekPremieresCommand(IMoviesInfoGetter infoGetter, IMovieInfoFormatter movieInfoFormatter) 
            : base("/weeknew", "премьеры недели")
        {
            this.movieInfoFormatter = movieInfoFormatter;
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string request)
        {
            var newMovies = moviesInfoGetter.GetWeekNewMovies().Select(movieInfoFormatter.Format);
            Logger.Debug("displayed week top");
            return string.Join("\r\n", newMovies);
        }
    }
}
