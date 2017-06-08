using System.Linq;

namespace CinemaGuideBot.BotCommands
{
    public class HelpCommand: BaseCommand<string>
    {
        public HelpCommand() : base("/help", "показывает это сообщение")
        {
        }

        public override string Execute(ICommandExecutor<string> invoker, string request)
        {
            Logger.Debug("displayed help");
            return GenerateHelp(invoker.GetAviableCommands());
        }

        public static string GenerateHelp(ICommand<string> [] commands)
        {
            var botCommands = commands.Select(command => $"{command.Name} - {command.HelpText}");
            return $"Я поддерживаю следующие команды:\r\n{string.Join("\r\n", botCommands)}";
        }
    }
}
