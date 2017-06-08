namespace CinemaGuideBot.BotCommands
{
    public interface ICommand<TResult>
    {
        TResult Execute(ICommandExecutor<TResult> invoker, string request);
        string HelpText { get; }
        string Name { get; }
    }
}
