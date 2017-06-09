using NLog;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public abstract class BaseCommand<T>: ICommand<T>
    {
        public string Name { get; }

        public string HelpText
            => typeof(BotReply).GetProperty($"{GetType().Name}Description").GetValue(Bot.BotReply).ToString();

        protected readonly Logger Logger;
        
        protected BaseCommand(string name)
        {
            Name = name;
            Logger = LogManager.GetLogger(GetType().Name);
        }

        public abstract T Execute(string request);
    }
}
