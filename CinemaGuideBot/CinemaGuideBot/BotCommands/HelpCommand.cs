using System.Linq;
using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class HelpCommand: BaseCommand
    {
        public HelpCommand() : base("/help", "show this help", "HelpCommand")
        {
        }

        public override void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter)
        {
            var helpText = GenerateHelp(botClient);
            botClient.SendTextMessageAsync(request.Chat.Id, helpText);
            Logger.Debug("for {0} displayed help", request.From.ToFormattedString());
        }

        public static string GenerateHelp(Bot botClient)
        {
            var botCommands = botClient.CommandExecutor.GetAviableCommands()
                .Select(command => $"{command.Name} - {command.HelpText}");
            return $"You can control me by sending these commands:\n{string.Join("\n", botCommands)}";
        }
    }
}
