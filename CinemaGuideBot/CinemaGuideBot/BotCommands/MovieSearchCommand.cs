using System;

namespace CinemaGuideBot.BotCommands
{
    public class MovieSearchCommand : BaseCommand<string>
    {
        public MovieSearchCommand() : base("/info", "поиск информации о фильме по названию")
        {
        }

        public override string Execute(ICommandExecutor<string> invoker, string searchTitle)
        {
            if (searchTitle == string.Empty)
                return "Введите название фильма";
            try
            {
                var result = invoker.MoviesInfoGetter.GetMovieInfo(searchTitle).ToString();

                if (string.IsNullOrEmpty(result))
                    throw new ArgumentException("Фильм не найден");

                Logger.Debug($"successfully found <{searchTitle}>");
                return result;
            }
            catch (ArgumentException e)
            {
                Logger.Debug($"not found <{searchTitle}>");
                Logger.Debug($"----------EXCEPTION----------\r\n{e}\r\n----------EXCEPTION----------");
                return $"Вы пытались найти \"{searchTitle}\"\r\n{e.Message}";
            }
        }
    }
}
