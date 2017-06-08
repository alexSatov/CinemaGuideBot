using NLog;
using System;
using System.Linq;
using Telegram.Bot.Types;
using System.Collections.Generic;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly Logger logger;
        private readonly IMoviesInfoGetter moviesInfoGetter;
        private readonly Dictionary<string, ICommand> commands;

        public CommandExecutor(IEnumerable<ICommand> commands, IMoviesInfoGetter moviesInfoGetter)
        {
            logger = LogManager.GetLogger(GetType().Name);
            this.moviesInfoGetter = moviesInfoGetter;
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
                try
                {
                    commandHandler.Execute(bot, message, moviesInfoGetter);
                }
                catch (Exception e)
                {
                    bot.SendTextMessageAsync(message.Chat.Id, "Непредвиденная ошибка");
                    logger.Debug($"----------EXCEPTION----------\r\n{e}\r\n----------EXCEPTION----------");
                }
            else
                bot.SendTextMessageAsync(message.Chat.Id, "Неизвестная команда");
        }
    }
}
