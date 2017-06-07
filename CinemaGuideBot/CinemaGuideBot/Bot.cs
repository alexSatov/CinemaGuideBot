using System.Diagnostics;
using CinemaGuideBot.BotCommands;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace CinemaGuideBot
{
    public class Bot : TelegramBotClient
    {
        private readonly Logger logger;
        public readonly ICommandExecutor CommandExecutor;
        public readonly string UserName;

        public Bot(string apiToken, ICommandExecutor executor) : base(apiToken)
        {
            CommandExecutor = executor;
            RegisterHandlers();
            var botInfo = GetMeAsync().Result;
            UserName = botInfo.Username;
            logger = LogManager.GetLogger($"Bot {UserName}");
            logger.Debug("bot successfully initialized");
        }

        private void RegisterHandlers()
        {
            OnCallbackQuery += BotOnCallbackQueryReceived;
            OnMessage += OnMessageReceived;
            OnReceiveError += ErrorHandler;
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

        private void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.TextMessage) return;
            CommandExecutor.Execute(this, message);
        }

        private async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            await AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                                           $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }
    }

}

