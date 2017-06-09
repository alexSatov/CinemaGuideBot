namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public interface ICommand<out T>
    {
        T Execute(string request);
        string HelpText { get; }
        string Name { get; }
    }
}
