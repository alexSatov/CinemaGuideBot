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
            : base("/info", "����� ���������� � ������ �� ��������")
        {
            this.movieInfoFormatter = movieInfoFormatter;
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string searchTitle)
        {
            if (searchTitle == string.Empty)
                return "������� �������� ������";
            try
            {
                var movieInfo = moviesInfoGetter.GetMovieInfo(searchTitle);
                var formattedInfo = movieInfoFormatter.Format(movieInfo);

                if (string.IsNullOrEmpty(formattedInfo))
                    throw new ArgumentException("����� �� ������");

                Logger.Debug($"successfully found <{searchTitle}>");
                return formattedInfo;
            }
            catch (ArgumentException e)
            {
                Logger.Debug($"not found <{searchTitle}>");
                return $"�� �������� ����� \"{searchTitle}\"\r\n{e.Message}";
            }
        }
    }
}
