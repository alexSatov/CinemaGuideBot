namespace CinemaGuideBot.BotCommands
{
    public class WeekTopCommand: BaseCommand<string>
    {
        public WeekTopCommand() : base("/weektop", "5 самых популярных фильмов недели")
        {
        }

        public override string Execute(ICommandExecutor<string> invoker, string request)
        {
            var topMovies = invoker.MoviesInfoGetter.GetWeekTopMovies();
            Logger.Debug("displayed week top");
            return string.Join("\r\n", topMovies);
        }
    }
}
