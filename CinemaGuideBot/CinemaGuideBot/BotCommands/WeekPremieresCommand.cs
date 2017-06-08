using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class WeekPremieresCommand : BaseCommand
    {
        public WeekPremieresCommand() : base("/new_week", "show new movies of week", "WeekPremieresCommand")
        {
        }
        public override void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter)
        {
            var newMovies = moviesInfoGetter.GetWeekNewMovies();
            botClient.SendTextMessageAsync(request.Chat.Id, string.Join("\r\n", newMovies));
            Logger.Debug("for {0} displayed week top", request.From.ToFormattedString());
        }
    }
}
