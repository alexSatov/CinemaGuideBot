using System;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public class StartCommand: BotCommand<string>
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
            return $"{Bot.BotReply.CurrentPhraseDict.Greeting}\r\n{helpText}";
        }
    }
}
