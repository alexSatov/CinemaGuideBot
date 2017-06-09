using System;
using CinemaGuideBot.Cinema.MoviesInfoGetters;

namespace CinemaGuideBot.TelegramBot.BotCommands
{
    public class MovieSearchCommand : BotCommand<string>
    {
        private readonly IMoviesInfoGetter moviesInfoGetter;

        public MovieSearchCommand(IMoviesInfoGetter infoGetter) : base("/info")
        {
            moviesInfoGetter = infoGetter;
        }

        public override string Execute(string searchTitle)
        {
            if (searchTitle == string.Empty)
                return Bot.BotReply.EnterMovieTitle;
            try
            {
                var movieInfo = moviesInfoGetter.GetMovieInfo(searchTitle);
                var formattedInfo = Bot.BotReply.ReplyFrom(movieInfo);

                if (string.IsNullOrEmpty(formattedInfo))
                    throw new ArgumentException("Movie not found (Empty info)");

                Logger.Debug($"successfully found <{searchTitle}>");
                return formattedInfo;
            }
            catch (ArgumentException e)
            {
                Logger.Warn($"not found <{searchTitle}> ({e.Message})");
                return Bot.BotReply.MovieNotFound;
            }
        }
    }
}
