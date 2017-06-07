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

        private static readonly Regex filmHrefExpr = new Regex(
            "<a href=\".+?/film/.+?\".+?data-url=\"(.+?)\".+?</a>", RegexOptions.Compiled);

        private static readonly Regex movieNotFoundExpr = new Regex(
            "К сожалению, по вашему запросу ничего не найдено", RegexOptions.Compiled);

        private static readonly Regex movieInfoExpr =
            new Regex(
                @"<h1 class=""moviename-big"".+?>(.+?)</h1>.+?<span itemprop=""alternativeHeadline"">(.+?)</span>.+?<td class=""type"">год</td>.+?>(\d+)</a>.+?<td class=""type"">страна</td>.*?<td.*?>(.+?)</td>.+?<td class=""type"">режиссер</td>.*?<td.*?>(.+?)</td>.+?<span class=""rating_ball"">(.+?)</span>.+?IMDb: ([\d.]+)",
                RegexOptions.Singleline & RegexOptions.Compiled);

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            var searchResultPage = WebPageParser.GetPage(KinopoiskUri, searchRequestPrefix + searchTitle);
            List<string> parseResult;

            if (WebPageParser.TryParsePage(searchResultPage, movieNotFoundExpr, out parseResult))
                throw new ArgumentException("Фильм не найден");

            if (!WebPageParser.TryParsePage(searchResultPage, filmHrefExpr, out parseResult))
                throw new Exception("Ошибка при получении ссылки на страницу с фильмом");

            var filmHref = parseResult[0];
            var filmPage = WebPageParser.GetPage(KinopoiskUri, filmHref);

            if (!WebPageParser.TryParsePage(filmPage, movieInfoExpr, out parseResult))
                throw new Exception("Ошибка при получении информации о фильме");

            var title = parseResult[0];
            var originalTitle = parseResult[1];
            var year = int.Parse(parseResult[2]);
            var country = WebPageParser.UniteParsedMultibleValues(parseResult[3]);
            var director = WebPageParser.UniteParsedMultibleValues(parseResult[4]);
            var rating = new Dictionary<string, double>
            {
                ["Кинопоиск"] = double.Parse(parseResult[5]),
                ["IMDb"] = double.Parse(parseResult[6])
            };

            return new MovieInfo
            {
                Country = country,
                Director = director,
                OriginalTitle = originalTitle,
                Rating = rating,
                Title = title,
                Year = year
            };
        }
    }
}