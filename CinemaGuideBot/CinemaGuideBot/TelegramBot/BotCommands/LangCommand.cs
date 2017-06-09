using System;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public class LangCommand : BotCommand<string>
    {
        public LangCommand() : base("/lang")
        {
        }

        public override string Execute(string language)
        {
            if (language == string.Empty)
                return Bot.BotReply.EnterLanguage;
            try
            {
                Bot.BotReply.SetLanguage(language);
                return Bot.BotReply.LanguageChanged;
            }
            catch (ArgumentException e)
            {
                Logger.Warn($"{e.Message}");
                return $"{Bot.BotReply.UnsupportedLanguage} {string.Join(", ", BotReply.SupportedLanguages)}";
            }
        }
    }
}