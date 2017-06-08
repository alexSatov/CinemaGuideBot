using System;
using Telegram.Bot.Types;
using CinemaGuideBot.Domain.MoviesInfoGetter;

namespace CinemaGuideBot.BotCommands
{
    public class MovieSearchCommand : BaseCommand
    {
        public MovieSearchCommand() : base("/info", "поиск информации о фильме по названию")
        {
        }

        public override void Execute(Bot botClient, Message request, IMoviesInfoGetter moviesInfoGetter)
        {
            var searchTitleStartIndex = request.Text.IndexOf(' ');

            if (searchTitleStartIndex == -1)
            {
                botClient.SendTextMessageAsync(request.Chat.Id, "Введите название фильма");
                return;
            }

            var searchTitle = request.Text.Substring(searchTitleStartIndex + 1);
            var sender = request.From.ToFormattedString();
            try
            {
                var result = moviesInfoGetter.GetMovieInfo(searchTitle).ToString();

                if (string.IsNullOrEmpty(result))
                    throw new ArgumentException("Фильм не найден");

                botClient.SendTextMessageAsync(request.Chat.Id, result);
                Logger.Debug($"for {sender} successfully found <{searchTitle}>");
            }
            catch (ArgumentException e)
            {
                botClient.SendTextMessageAsync(request.Chat.Id, $"Вы пытались найти \"{searchTitle}\"\r\n{e.Message}");
                Logger.Debug($"for {sender} not found <{searchTitle}>");
            }
        }
    }
}
