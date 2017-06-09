using System;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class MovieSearchCommand : BaseCommand
    {
        private readonly IMoviesInfoGetter moviesInfoGetter;

        public MovieSearchCommand(IMoviesInfoGetter infoGetter) : base("/info", "поиск информации о фильме по названию")
        {
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string searchTitle)
        {
            if (searchTitle == string.Empty)
                return "Введите название фильма";
            try
            {
                var result = moviesInfoGetter.GetMovieInfo(searchTitle).ToString();

                if (string.IsNullOrEmpty(result))
                    throw new ArgumentException("Фильм не найден");

                Logger.Debug($"successfully found <{searchTitle}>");
                return result;
            }
            catch (ArgumentException e)
            {
                Logger.Debug($"not found <{searchTitle}>");
                return $"Вы пытались найти \"{searchTitle}\"\r\n{e.Message}";
            }
        }
    }
}
