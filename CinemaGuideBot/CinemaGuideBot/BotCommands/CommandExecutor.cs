using NLog;
using System.Linq;
using Telegram.Bot.Types;
using System.Collections.Generic;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class CommandExecutor: ICommandExecutor
    {
        private readonly Logger logger;
        private readonly Dictionary<string, ICommand> commands;
        private readonly IMoviesInfoGetter moviesInfoGetter;

        public CommandExecutor(ICommand[] commands, IMoviesInfoGetter moviesInfoGetter)
        {
            this.moviesInfoGetter = moviesInfoGetter;
            logger = LogManager.GetLogger("CommandExecutor");
            this.commands = commands.ToDictionary(command => command.Name, command => command);
            logger.Debug("added new commands: {0}", string.Join(", ", this.commands.Keys));
        }

        public ICommand[] GetAviableCommands()
        {
            return commands.Values.ToArray();
        }

        public void Register(ICommand newCommand)
        {
            if (commands.ContainsKey(newCommand.Name)) return;
            commands.Add(newCommand.Name, newCommand);
            logger.Debug("added new command: {0}", newCommand.Name);

        }

        public void Execute(Bot bot, Message message)
        {
            ICommand commandHandler;
            var command = message.Text.Split().First();
            if (commands.TryGetValue(command, out commandHandler))
            {
                commandHandler.Execute(bot, message, moviesInfoGetter);
            }
            else
            {
                bot.SendTextMessageAsync(message.Chat.Id, "I'm sorry but I don't understand your command");
            }
        }
    }
}
