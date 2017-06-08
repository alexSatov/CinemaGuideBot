using NLog;
using System;
using System.Linq;
using Telegram.Bot.Types;
using System.Collections.Generic;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class CommandExecutor : ICommandExecutor<String>
    {
        public IMoviesInfoGetter MoviesInfoGetter { get; set; }
        private readonly Logger logger;
        private readonly Dictionary<string, ICommand<String>> commands;
        public CommandExecutor(ICommand<String>[] commands, IMoviesInfoGetter moviesInfoGetter)
        {
            logger = LogManager.GetLogger(GetType().Name);
            this.MoviesInfoGetter = moviesInfoGetter;
            this.commands = commands.ToDictionary(command => command.Name, command => command);
            logger.Debug("added new commands: {0}", string.Join(", ", this.commands.Keys));
        }

        public ICommand<string>[] GetAviableCommands()
        {
            return commands.Values.ToArray();
        }

        public void Register(ICommand<string> newCommand)
        {
            if (commands.ContainsKey(newCommand.Name)) return;
            commands.Add(newCommand.Name, newCommand);
            logger.Debug("added new command: {0}", newCommand.Name);
        }

        public void Execute(Bot bot, Message message)
        {
            ICommand<String> commandHandler;
            var args = message.Text.Split();
            if (commands.TryGetValue(args.First(), out commandHandler))
                try
                {
                    var request = string.Join("", args.Skip(1));
                    var result = commandHandler.Execute(this, request);
                    bot.SendTextMessageAsync(message.From.Id, result);
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
