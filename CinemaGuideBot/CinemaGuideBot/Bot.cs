using NLog;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Diagnostics;
using Telegram.Bot.Types.Enums;
using CinemaGuideBot.BotCommands;

namespace CinemaGuideBot
{
    public class Bot : TelegramBotClient
    {
        public readonly ICommandExecutor CommandExecutor;
        public readonly string UserName;

        private readonly Logger logger;

        public Bot(string token, ICommandExecutor executor) : base(token)
        {
            CommandExecutor = executor;
            RegisterHandlers();
            var botInfo = GetMeAsync().Result;
            UserName = botInfo.Username;
            logger = LogManager.GetLogger($"{GetType().Name} {UserName}");
            logger.Debug("bot successfully initialized");
        }

        private void RegisterHandlers()
        {
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

            if (message == null || message.Type != MessageType.TextMessage)
                return;

            CommandExecutor.Execute(this, message);
        }
    }
}

