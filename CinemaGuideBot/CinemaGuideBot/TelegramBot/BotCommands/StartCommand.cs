using System;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public class StartCommand: BaseCommand<string>
    {
        private readonly Lazy<ICommandExecutor<string>> commandExecutor;

        public StartCommand(Lazy<ICommandExecutor<string>> commandExecutor) : base("/start")
        {
            this.commandExecutor = commandExecutor;
        }

        public override string Execute(string request)
        {
            var aviableCommands = commandExecutor.Value.GetAviableCommands();
            var helpText = HelpCommand.GenerateHelp(aviableCommands);
            Logger.Debug("displayed start message");
            return $"{Bot.BotReply.Greeting}\r\n{helpText}";
        }
    }
}
