using System;
using System.Collections.Generic;
using CinemaGuideBot.Infrastructure;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Domain
{
    public class KinopoiskWebPageMIG : IMovieInfoGetter
    {
        public static readonly Uri KinopoiskUri = new Uri("https://www.kinopoisk.ru");

        private const string searchRequestPrefix = "/index.php?first=no&what=&kp_query=";

        private static readonly Regex movieIdExpr = new Regex("<a href=\".+?/film/(.+?)/", RegexOptions.Compiled);
        private static readonly Regex movieNotFoundExpr = new Regex("К сожалению, по вашему запросу ничего не найдено", RegexOptions.Compiled);

        private static readonly Regex movieInfoExpr =
            new Regex(
                @"<h1 class=""moviename-big"".+?>(.+?)</h1>.+?<span itemprop=""alternativeHeadline"">(.+?)</span>.+?<td class=""type"">год</td>.+?>(\d+)</a>.+?<td class=""type"">страна</td>.*?<td.*?>(.+?)</td>.+?<td class=""type"">режиссер</td>.*?<td.*?>(.+?)</td>.+?<span class=""rating_ball"">(.+?)</span>.+?IMDb: ([\d.]+)",
                RegexOptions.Singleline | RegexOptions.Compiled);

        public static int GetMovieId(string title)
        {
            var searchResultPage = WebPageParser.GetPage(KinopoiskUri, searchRequestPrefix + title);
            List<string> parseResult;

            if (WebPageParser.TryParsePage(searchResultPage, movieNotFoundExpr, out parseResult))
                throw new ArgumentException("Фильм не найден");

            if (!WebPageParser.TryParsePage(searchResultPage, movieIdExpr, out parseResult))
                throw new Exception("Ошибка при получении id фильма");

            return int.Parse(parseResult[0]);
        }

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            List<string> parseResult;
            var filmHref = $"/film/{GetMovieId(searchTitle)}/";
            var filmPage = WebPageParser.GetPage(KinopoiskUri, filmHref);

            if (!WebPageParser.TryParsePage(filmPage, movieInfoExpr, out parseResult))
                throw new Exception("Ошибка при получении информации о фильме");
          
            var rating = new Dictionary<string, double>
            {
                ["Кинопоиск"] = double.Parse(parseResult[5].Replace('.', ',')),
                ["IMDb"] = double.Parse(parseResult[6].Replace('.', ','))
            };

            return new MovieInfo
            {
                Title = parseResult[0],
                OriginalTitle = parseResult[1],
                Year = int.Parse(parseResult[2]),
                Country = WebPageParser.UniteParsedMultibleValues(parseResult[3]),
                Director = WebPageParser.UniteParsedMultibleValues(parseResult[4]),
                Rating = rating
            };
        }
    }
}