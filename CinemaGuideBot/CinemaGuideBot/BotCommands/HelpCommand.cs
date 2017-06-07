using System.Linq;
using CinemaGuideBot.Domain;
using NLog;
using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public class HelpCommand: ICommand
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
       
        public void Execute(Bot botClient, Message request, IMovieInfoGetter movieInfoGetter)
        {
            var helpText = GenerateHelp(botClient);
            botClient.SendTextMessageAsync(request.Chat.Id, helpText);
            logger.Debug("for client({0}) of {1} displayed help", request.From.ToFormattedString(), botClient.UserName);
        }

        public static string GenerateHelp(Bot botClient)
        {
            var botCommands = botClient.CommandExecutor.GetAviableCommands()
                .Select(command => $"{command.Name} - {command.HelpText}");
            return $"You can control me by sending these commands:\n{string.Join("\n", botCommands)}";
        }

        public string HelpText => "show this help";

        public string Name => "/help";
    }
}
