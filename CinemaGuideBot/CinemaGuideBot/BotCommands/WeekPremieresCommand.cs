namespace CinemaGuideBot.BotCommands
{
    public class WeekPremieresCommand : BaseCommand<string>
    {
        public WeekPremieresCommand() : base("/weeknew", "премьеры недели")
        {
        }

        public override string Execute(ICommandExecutor<string> invoker, string request)
        {
            var newMovies = invoker.MoviesInfoGetter.GetWeekNewMovies();
            Logger.Debug("displayed week top");
            return string.Join("\r\n", newMovies);
        }
    }
}
