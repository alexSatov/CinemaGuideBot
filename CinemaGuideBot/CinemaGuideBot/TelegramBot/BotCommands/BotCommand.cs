using NLog;
using CinemaGuideBot.TelegramBot.Localisation;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public abstract class BotCommand<T>: ICommand<T>
    {
        public string Name { get; }

        public string HelpText => typeof(IPhraseDict)
            .GetProperty($"{GetType().Name}Description")?
            .GetValue(Bot.BotReply.CurrentPhraseDict)?
            .ToString();

        protected readonly Logger Logger;
        
        protected BotCommand(string name)
        {
            Name = name;
            Logger = LogManager.GetLogger(GetType().Name);
        }

        public abstract T Execute(string request);
    }
}
