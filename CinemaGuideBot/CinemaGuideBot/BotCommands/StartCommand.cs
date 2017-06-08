using NLog;
using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MovieInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    class StartCommand: ICommand
    {
        private static readonly Logger logger = LogManager.GetLogger("StartCommand");

        public void Execute(Bot botClient, Message request, IMovieInfoGetter movieInfoGetter)
        {

            var helpText = HelpCommand.GenerateHelp(botClient);
            var startText = $"Hello, dear user! I am your guide in cinema world.\n{helpText}";
            botClient.SendTextMessageAsync(request.Chat.Id, startText);
            logger.Debug("for {0} displayed start message", request.From.ToFormattedString());
        }

        public string HelpText => "show start info";
        public string Name => "/start";
    }
}
