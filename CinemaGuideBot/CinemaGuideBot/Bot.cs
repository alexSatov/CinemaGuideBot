using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CinemaGuideBot.BotCommands;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace CinemaGuideBot
{
    public class Bot : TelegramBotClient
    {
        private readonly Dictionary<string, ICommand> commands;
        private readonly Logger logger;  
        public readonly string UserName;

        public Bot(string apiToken, params ICommand[] botCommands) : base(apiToken)
        {
            RegisterHandlers();
            var botInfo = GetMeAsync().Result;
            UserName = botInfo.Username;
            logger = LogManager.GetLogger($"Bot {UserName}");
            logger.Debug("bot successfully initialized");
            commands = botCommands.ToDictionary(command => command.Name, command => command);
            logger.Debug("added new commands: {0}", string.Join(", ", commands.Keys));
        }

        private void RegisterHandlers()
        {
            OnCallbackQuery += BotOnCallbackQueryReceived;
            OnMessage += OnMessageReceived;
            OnReceiveError += ErrorHandler;
        }

        public ICommand[] GetCommands()
        {
            return commands.Values.ToArray();
        }

        public void AddCommand(ICommand newCommand)
        {
            if (commands.ContainsKey(newCommand.Name)) return;
            commands.Add(newCommand.Name, newCommand);
            logger.Debug("added new command: {0}", newCommand.Name);
        }

        public void StartWorking()
        {
            StartReceiving();
            logger.Debug("bot began work");
        }

        public void StopWorking()
        { 
            StopReceiving();
            logger.Debug("bot completed the work");
        }

        private static void ErrorHandler(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Debugger.Break();
        }

        private async void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.TextMessage) return;
            var command = message.Text.Split().First();
            ICommand commandHandler;
            if (commands.TryGetValue(command, out commandHandler))
            {
                commandHandler.Execute(this, message);
            }
            else
            {
                await SendTextMessageAsync(message.Chat.Id, "I'm sorry but I don't understand your command");
            }

        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                                           $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }
    }

}

