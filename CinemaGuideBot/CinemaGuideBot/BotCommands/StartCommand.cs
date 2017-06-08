using Telegram.Bot.Types; 
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class StartCommand: BaseCommand
    {
        public StartCommand() : base("/start", "приветствие и help")
        {
        }

        public override void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter)
        {
            var helpText = HelpCommand.GenerateHelp(botClient);
            var startText = $"Приветствую! Я твой гид в мире кино. Давай же начнем.\r\n{helpText}";
            botClient.SendTextMessageAsync(request.Chat.Id, startText);
            Logger.Debug("for {0} displayed start message", request.From.ToFormattedString());
        }
    }
}
