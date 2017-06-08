namespace CinemaGuideBot.BotCommands
{
    public class StartCommand: BaseCommand<string>
    {
        public StartCommand() : base("/start", "приветствие и help")
        {
        }

        public override string Execute(ICommandExecutor<string> invoker, string request)
        {
            var helpText = HelpCommand.GenerateHelp(invoker.GetAviableCommands());
            Logger.Debug("displayed start message");
            return $"Приветствую! Я твой гид в мире кино. Давай же начнем.\r\n{helpText}";
        }
    }
}
