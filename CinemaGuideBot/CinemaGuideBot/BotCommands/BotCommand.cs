using NLog;

namespace CinemaGuideBot.BotCommands
{
    public abstract class BaseCommand<T>: ICommand<T>
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

        public abstract T Execute(string request);
    }
}
