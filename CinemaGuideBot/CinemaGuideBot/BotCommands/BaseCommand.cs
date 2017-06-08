using NLog;

namespace CinemaGuideBot.BotCommands
{
    public abstract class BaseCommand<TResult> : ICommand<TResult>
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

        public abstract TResult Execute(ICommandExecutor<TResult> invoker, string request);
    }
}
