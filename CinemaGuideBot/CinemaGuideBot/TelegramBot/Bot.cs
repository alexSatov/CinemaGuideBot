using NLog;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using CinemaGuideBot.TelegramBot.BotCommands;

namespace CinemaGuideBot.TelegramBot
{
    public class Bot : TelegramBotClient
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(Bot));
        public readonly ICommandExecutor<string> CommandExecutor;
        public readonly string Name;

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

        private void ErrorHandler(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            logger.Fatal(receiveErrorEventArgs.ToString());
            StopReceiving();
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