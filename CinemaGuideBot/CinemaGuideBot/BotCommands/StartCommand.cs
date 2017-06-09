using System;

namespace CinemaGuideBot.BotCommands
{
    public class StartCommand: BaseCommand
    {
        private readonly Lazy<ICommandExecutor> commandExecutor;

        public StartCommand(Lazy<ICommandExecutor> commandExecutor) : base("/start", "приветствие и help")
        {
            this.commandExecutor = commandExecutor;
        }

        public override string Execute(string request)
        {
            var aviableCommands = commandExecutor.Value.GetAviableCommands();
            var helpText = HelpCommand.GenerateHelp(aviableCommands);
            Logger.Debug("displayed start message");
            return $"Приветствую! Я твой гид в мире кино. Давай же начнем.\r\n{helpText}";
        }
    }
}
