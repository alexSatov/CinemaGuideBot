using System;
using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MovieInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    class MovieSearchCommand : ICommand
    {
        public void Execute(Bot botClient, Message request, IMovieInfoGetter movieInfoGetter)
        {
            var movieTitleStartIndex = request.Text.IndexOf(' ');
            if(movieTitleStartIndex == -1) return;
            var movieTitle = request.Text.Substring(movieTitleStartIndex + 1);
            try
            {
                var result = movieInfoGetter.GetMovieInfo(movieTitle);
                botClient.SendTextMessageAsync(request.Chat.Id, result.ToString());
            }
            catch (ArgumentException)
            {
                botClient.SendTextMessageAsync(request.Chat.Id, "Sorry, but i can't find movie by this title :(");
            }
        }

        public string HelpText => "search information about movie";
        public string Name => "/movieInfo";
    }
}
