namespace CinemaGuideBot.BotCommands
{
    public interface ICommand
    {
        string Execute(string request);
        string HelpText { get; }
        string Name { get; }
    }
}
