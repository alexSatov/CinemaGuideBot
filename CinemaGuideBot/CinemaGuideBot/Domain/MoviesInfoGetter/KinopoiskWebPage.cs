using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CinemaGuideBot.Infrastructure;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Domain.MoviesInfoGetter
{
    public class KinopoiskWebPage : IMoviesInfoGetter
    {
        public static readonly Uri KinopoiskUri = new Uri("https://www.kinopoisk.ru");

        private const string searchRequestPrefix = "/index.php?first=no&what=&kp_query=";
        
        private static readonly Regex movieIdExpr = new Regex(@"<a href="".*?/film/(\d+?)/", RegexOptions.Compiled);

        private static readonly Regex weekTopMoviesExpr = new Regex(
            "<dl class=\"block block_cash\" id=\"rigth_box_weekend_rus\".+?</dl>",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex weekPremieresExpr = new Regex(
            "<div class=\"prem_list\">.+?<div class=\"prem_list\">",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex movieNotFoundExpr = new Regex(
            "К сожалению, по вашему запросу ничего не найдено",
            RegexOptions.Compiled);

        //private static readonly Regex textValueExpr = new Regex(@">([\d\w\s]+?)<", RegexOptions.Compiled);
        //private static readonly Regex movieInfoExpr = new Regex(  
        //    @"<h1 class=""moviename-big"".+?>(.+?)</h1>.+?
        //      <span itemprop=""alternativeHeadline"">(.+?)</span>.+?
        //      <td class=""type"">год</td>.+?>(\d+)</a>.+?
        //      <td class=""type"">страна</td>.*?<td.*?>(.+?)</td>.+?
        //      <td class=""type"">режиссер</td>.*?<td.*?>(.+?)</td>.+?
        //      <span class=""rating_ball"">(.+?)</span>.+?
        //      IMDb: ([\d.]+)",
        //    RegexOptions.Compiled | RegexOptions.Singleline);

        public static async Task<string> GetMovieSearchPageAsync(string title)
        {
            List<string> parseResult;
            var searchResultPage = await WebPageParser.GetPageAsync(KinopoiskUri, searchRequestPrefix + title);

            if (WebPageParser.TryParsePage(searchResultPage, movieNotFoundExpr, out parseResult))
                throw new ArgumentException("Фильм не найден");

            return searchResultPage;
        }

        public static List<int> GetMoviesId(string pageElement)
        {
            return WebPageParser
                .UniteParsedMultibleValues(pageElement, movieIdExpr)
                .Select(int.Parse)
                .Distinct()
                .ToList();
        }

        public static string GetCashBlock()
        {
            return GetPageBlock("/", weekTopMoviesExpr);
        }

        public static string GetWeekPremieresBlock()
        {
            return GetPageBlock("/premiere/ru/", weekPremieresExpr);
        }

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            throw new NotImplementedException();
        }

        public List<MovieInfo> GetWeekTopMovies()
        {
            throw new NotImplementedException();
        }

        public List<MovieInfo> GetWeekNewMovies()
        {
            throw new NotImplementedException();
        }

        private static string GetPageBlock(string request, Regex pageBlockExpr,
            string error = "Возникла ошибка при получении статистики")
        {
            List<string> parseResult;
            var page = WebPageParser.GetPageAsync(KinopoiskUri, request).Result;

            if (!WebPageParser.TryParsePage(page, pageBlockExpr, out parseResult))
                throw new ArgumentException(error);

            return parseResult[0];
        }

        //public MovieInfo GetMovieInfo(string searchTitle)
        //{
        //    List<string> parseResult;
        //    var movieSearchPage = GetMovieSearchPageAsync(searchTitle).Result;
        //    var filmHref = $"/film/{GetMoviesId(movieSearchPage)[0]}/";
        //    var filmPage = WebPageParser.GetPageAsync(KinopoiskUri, filmHref).Result;

        //    if (!WebPageParser.TryParsePage(filmPage, movieInfoExpr, out parseResult))
        //        throw new Exception("Возникла ошибка при получении информации о фильме");

        //    var country = string.Join(", ", WebPageParser.UniteParsedMultibleValues(parseResult[4], textValueExpr));
        //    var director = string.Join(", ", WebPageParser.UniteParsedMultibleValues(parseResult[5], textValueExpr));

        //    var rating = new Dictionary<string, string>
        //    {
        //        ["Кинопоиск"] = parseResult[6],
        //        ["IMDb"] = parseResult[7]
        //    };

        //    return new MovieInfo
        //    {
        //        Title = parseResult[1],
        //        OriginalTitle = parseResult[2],
        //        Year = int.Parse(parseResult[3]),
        //        Country = country,
        //        Director = director,
        //        Rating = rating
        //    };
        //}
    }
}