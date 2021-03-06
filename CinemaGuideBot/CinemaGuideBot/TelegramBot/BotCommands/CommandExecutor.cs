using NLog;
using System;
using System.Linq;
using Telegram.Bot.Types;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public class CommandExecutor : ICommandExecutor<string>
    {
        private readonly Logger logger;
        private readonly Dictionary<string, ICommand<string>> commands;
        private static readonly Regex textCommandParser = new Regex(@"(?<commandName>/\w+)\s?(?<request>.*)", RegexOptions.Compiled);

        public CommandExecutor(IEnumerable<ICommand<string>> commands)
        {
            logger = LogManager.GetLogger(GetType().Name);
            this.commands = commands.ToDictionary(command => command.Name, command => command);
            logger.Debug("added new commands: {0}", string.Join(", ", this.commands.Keys));
        }

        public ICommand<string>[] GetAviableCommands()
        {
            return commands.Values.ToArray();
        }

        public void Register(params ICommand<string>[] newCommands)
        {
            foreach (var newCommand in newCommands)
            {
                if (commands.ContainsKey(newCommand.Name)) continue;
                commands.Add(newCommand.Name, newCommand);
                logger.Debug("added new command: {0}", newCommand.Name);
            }
        }

        public string Execute(Message message)
        {
            ICommand<string> commandHandler;
            var match = textCommandParser.Match(message.Text);
            var coomandName = match.Groups["commandName"].Value;

            if (!commands.TryGetValue(coomandName, out commandHandler))
                throw new ArgumentOutOfRangeException($"Unknown command {coomandName}");

            var request = match.Groups["request"].Value;
            var response = commandHandler.Execute(request);
            return response;
        }
    }
}
