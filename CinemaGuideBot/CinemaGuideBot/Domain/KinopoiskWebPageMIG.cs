using System;
using System.Collections.Generic;
using CinemaGuideBot.Infrastructure;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Domain
{
    public class KinopoiskWebPageMIG : IMovieInfoGetter
    {
        public static readonly Uri KinopoiskUri = new Uri("https://www.kinopoisk.ru/");

        private const string searchRequestPrefix = "index.php?first=no&what=&kp_query=";

        private static readonly Regex valueExpr = new Regex(@">[\w\s]+?<");
        private static readonly Regex filmHrefExpr = new Regex("<a href=\".+?/film/.+?\".+?data-url=\"(.+?)\".+?</a>");
        private static readonly Regex movieNotFoundExpr = new Regex("К сожалению, по вашему запросу ничего не найдено");

        private static readonly Regex movieInfo = new Regex("<h1 class=\"moviename-big\".+?>(.+?)</h1>.+?" +
                                                            "<span itemprop=\"alternativeHeadline\">(.+?)</span>.+?" +
                                                            "<td class=\"type\">страна</td>.*?<td.*?>(.+?)</td>.+?" +
                                                            "<td class=\"type\">режиссер</td>.*?<td.*?>(.+?)</td>.+?" +
                                                            "<span class=\"rating_ball\">(.+?)</span>.+?" +
                                                            @"IMDb: ([\d.]+)");

        public MovieInfo[] GetMovieInfo(string title)
        {
            var searchResultPage = WebPageParser.GetPage(KinopoiskUri, searchRequestPrefix + title);
            List<string> parseResult;

            if (WebPageParser.TryParsePage(searchResultPage, movieNotFoundExpr, out parseResult))
                throw new ArgumentException("Фильм не найден");

            if (!WebPageParser.TryParsePage(searchResultPage, filmHrefExpr, out parseResult))
                throw new Exception("Ошибка при получении ссылки на страницу с фильмом");

            var filmHref = parseResult[0];
            var filmPage = WebPageParser.GetPage(KinopoiskUri, filmHref);
        }
    }
}