using Telegram.Bot.Types;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public static class UserExtension
    {
        public static string ToFormattedString(this User user)
        {
            return $"{user.LastName} {user.FirstName}(id={user.Id})";
        }
    }
}