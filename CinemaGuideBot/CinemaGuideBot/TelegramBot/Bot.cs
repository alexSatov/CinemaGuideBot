using NLog;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using CinemaGuideBot.TelegramBot.BotCommands;

namespace CinemaGuideBot.TelegramBot
{
    public class Bot : TelegramBotClient
    {
        public readonly string Name;
        public static BotReply BotReply { get; private set; }
        public readonly ICommandExecutor<string> CommandExecutor;

        private static readonly Logger logger = LogManager.GetLogger(nameof(Bot));

        public Bot(string token, ICommandExecutor<string> executor, BotReply botReply) : base(token)
        {
            CommandExecutor = executor;
            RegisterHandlers();
            Name = GetBotName();
            BotReply = botReply;
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

            string reply;
            try
            {
                reply = CommandExecutor.Execute(message);
            }
            catch (ArgumentOutOfRangeException e)
            {
                logger.Debug(e);
                reply = BotReply.UnknownCommand;
            }
            catch (Exception e)
            {
                logger.Error(e);
                reply = BotReply.UnexpectedError;
            }

            SendTextMessageAsync(message.Chat.Id, reply);
        }
    }
}