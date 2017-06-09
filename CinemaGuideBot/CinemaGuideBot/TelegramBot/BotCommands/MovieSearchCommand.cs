using System;
using CinemaGuideBot.Cinema.MoviesInfoGetters;
using CinemaGuideBot.Cinema.MovieInfoFormatters;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public class MovieSearchCommand : BaseCommand<string>
    {
        private readonly IMoviesInfoGetter moviesInfoGetter;
        private readonly IMovieInfoFormatter movieInfoFormatter;
        public MovieSearchCommand(IMoviesInfoGetter infoGetter, IMovieInfoFormatter movieInfoFormatter) 
            : base("/info", "поиск информации о фильме по названию")
        {
            this.movieInfoFormatter = movieInfoFormatter;
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string searchTitle)
        {
            if (searchTitle == string.Empty)
                return "Введите название фильма";
            try
            {
                var movieInfo = moviesInfoGetter.GetMovieInfo(searchTitle);
                var formattedInfo = movieInfoFormatter.Format(movieInfo);

                if (string.IsNullOrEmpty(formattedInfo))
                    throw new ArgumentException("Фильм не найден");

                Logger.Debug($"successfully found <{searchTitle}>");
                return formattedInfo;
            }
            catch (ArgumentException e)
            {
                Logger.Debug($"not found <{searchTitle}>");
                return $"Вы пытались найти \"{searchTitle}\"\r\n{e.Message}";
            }
        }
    }
}
