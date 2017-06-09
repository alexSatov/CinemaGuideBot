using NLog;
using System;
using System.Linq;
using Telegram.Bot.Types;
using System.Collections.Generic;

namespace CinemaGuideBot.BotCommands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly Logger logger;
        private readonly Dictionary<string, ICommand> commands;

        public CommandExecutor(ICommand[] commands)
        {
            logger = LogManager.GetLogger(GetType().Name);
            this.commands = commands.ToDictionary(command => command.Name, command => command);
            logger.Debug("added new commands: {0}", string.Join(", ", this.commands.Keys));
        }

        public ICommand[] GetAviableCommands()
        {
            return commands.Values.ToArray();
        }

        public void Register(params ICommand[] newCommands)
        {
            foreach (var newCommand in newCommands)
            {
                if (commands.ContainsKey(newCommand.Name)) continue;
                commands.Add(newCommand.Name, newCommand);
                logger.Debug("added new command: {0}", newCommand.Name);
            }
        }

        public void Execute(Bot bot, Message message)
        {
            ICommand commandHandler;
            var args = message.Text.Split();
            if (commands.TryGetValue(args.First(), out commandHandler))
                try
                {
                    var request = string.Join("", args.Skip(1));
                    var result = commandHandler.Execute(request);
                    bot.SendTextMessageAsync(message.From.Id, result);
                }
                catch (Exception e)
                {
                    bot.SendTextMessageAsync(message.Chat.Id, "Непредвиденная ошибка");
                    logger.Error(e);
                }
            else
                bot.SendTextMessageAsync(message.Chat.Id, "Неизвестная команда");
        }
    }
}
