using System;
using System.Linq;

namespace CinemaGuideBot.BotCommands
{
    public class HelpCommand: BaseCommand
    {
        private readonly Lazy<ICommandExecutor> commandExecutor;

        public HelpCommand(Lazy<ICommandExecutor> commandExecutor) : base("/help", "показывает это сообщение")
        {
            this.commandExecutor = commandExecutor;
        }

        public override string Execute(string request)
        {
            Logger.Debug("displayed help");
            return GenerateHelp(commandExecutor.Value.GetAviableCommands());
        }

        public static string GenerateHelp(ICommand[] commands)
        {
            var botCommands = commands.Select(command => $"{command.Name} - {command.HelpText}");
            return $"Я поддерживаю следующие команды:\r\n{string.Join("\r\n", botCommands)}";
        }
    }
}
