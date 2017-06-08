using Telegram.Bot.Types; 
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class StartCommand: BaseCommand
    {
        public StartCommand() : base("/start", "show start info", "StartCommand")
        {
        }

        public override void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter)
        {
            var helpText = HelpCommand.GenerateHelp(botClient);
            var startText = $"Hello, dear user! I am your guide in cinema world.\n{helpText}";
            botClient.SendTextMessageAsync(request.Chat.Id, startText);
            logger.Debug("for {0} displayed start message", request.From.ToFormattedString());
        }
    }
}
