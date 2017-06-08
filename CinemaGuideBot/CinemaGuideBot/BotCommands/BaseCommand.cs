using NLog;
using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public abstract class BaseCommand : ICommand
    {
        public string Name { get; }
        public string HelpText { get; }

        protected readonly Logger Logger;
        
        protected BaseCommand(string name, string helpText)
        {
            Name = name;
            HelpText = helpText;
            Logger = LogManager.GetLogger(GetType().Name);
        }

        public abstract void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter);
    }
}
