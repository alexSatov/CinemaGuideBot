using System;
using System.Linq;
using CinemaGuideBot.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Cinema.MoviesInfoGetters
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
            "  сожалению, по вашему запросу ничего не найдено",
            RegexOptions.Compiled);

        public static async Task<string> GetMovieSearchPageAsync(string title)
        {
            List<string> parseResult;
            var searchResultPage = await WebPageParser.GetPageAsync(KinopoiskUri, searchRequestPrefix + title);

            if (WebPageParser.TryParsePage(searchResultPage, movieNotFoundExpr, out parseResult))
                throw new ArgumentException("Movie not found");

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
            string error = "Error while parsing page block")
        {
            List<string> parseResult;
            var page = WebPageParser.GetPageAsync(KinopoiskUri, request).Result;

            if (!WebPageParser.TryParsePage(page, pageBlockExpr, out parseResult))
                throw new ArgumentException(error);

            return parseResult[0];
        }
    }
}