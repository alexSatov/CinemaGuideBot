using NLog;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using CinemaGuideBot.BotCommands;

namespace CinemaGuideBot
{
    public class Bot : TelegramBotClient
    {
        public readonly ICommandExecutor<string> CommandExecutor;
        public readonly string Name;
        private static readonly Logger logger = LogManager.GetLogger(nameof(Bot));

        public Bot(string token, ICommandExecutor<string> executor) : base(token)
        {
            CommandExecutor = executor;
            RegisterHandlers();
            Name = GetBotName();
        }

        private string GetBotName()
        {
            var botInfo = GetMeAsync().Result;
            return botInfo.Username;
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

        private void ErrorHandler(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            logger.Fatal(receiveErrorEventArgs.ToString());
            StopWorking();
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

