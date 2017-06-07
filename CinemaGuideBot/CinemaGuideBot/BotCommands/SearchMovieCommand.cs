using System;
using CinemaGuideBot.Domain;
using Telegram.Bot.Types;

namespace CinemaGuideBot.BotCommands
{
    class SearchMovieCommand : ICommand
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
            catch (ArgumentException e)
            {
                botClient.SendTextMessageAsync(request.Chat.Id, "Sorry, but i can't find movie by this title :(");
            }
        }

        public string HelpText => "search information about movie";
        public string Name => "/movieInfo";
    }
}
