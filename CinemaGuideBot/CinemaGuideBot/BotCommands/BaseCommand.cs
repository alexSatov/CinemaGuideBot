using CinemaGuideBot.Domain.MoviesInfoGetter;
using NLog;
using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public abstract class BaseCommand : ICommand
    {
        public string HelpText { get; }
        public string Name { get; }
        protected readonly Logger Logger;
        
        protected BaseCommand(string name, string helpText, string loggerName)
        {
            Logger = LogManager.GetLogger(loggerName);
            HelpText = helpText;
            Name = name;
        }

        public abstract void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter);
       
    }
}
