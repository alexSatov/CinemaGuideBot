using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public static class UserExtension{
        public static string ToFormattedString(this User user)
        {
            return $"{user.LastName} {user.FirstName}(id={user.Id})";
        }
    }
}