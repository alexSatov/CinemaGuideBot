using NLog;
using System;
using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    class MovieSearchCommand : ICommand
    {
        private static readonly Logger logger = LogManager.GetLogger("MovieSearchCommand");
        public void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter)
        {
            var movieTitleStartIndex = request.Text.IndexOf(' ');
            if(movieTitleStartIndex == -1) return;
            var movieTitle = request.Text.Substring(movieTitleStartIndex + 1);
            try
            {
                var result = moviesInfoGetter.GetMovieInfo(movieTitle);
                botClient.SendTextMessageAsync(request.Chat.Id, result.ToString());
                logger.Debug("for {0} successfully found <{1}>",
                    request.From.ToFormattedString(), movieTitle);
            }
            catch (ArgumentException)
            {
                botClient.SendTextMessageAsync(request.Chat.Id, "Sorry, but i can't find movie by this title :(");
                logger.Debug("for {0} not found <{1}>",
                    request.From.ToFormattedString(), movieTitle);
            }
        }

        public string HelpText => "search information about movie";
        public string Name => "/info";
    }
}
