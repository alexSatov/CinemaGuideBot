using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    public static class UserExtension{
        public static string ToFormattedString(this User user)
        {
            return $"Id: {user.Id}, First name: {user.FirstName}, Last name: {user.LastName}";
        }
    }
}