using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CinemaGuideBot.Infrastructure;

namespace CinemaGuideBot.Domain.MoviesInfoGetter
{
    public class KinopoiskWebPage : IMoviesInfoGetter
    {
        public static readonly Uri KinopoiskUri = new Uri("https://www.kinopoisk.ru");

        private const string searchRequestPrefix = "/index.php?first=no&what=&kp_query=";

        private static readonly Regex movieIdExpr = new Regex("<a href=\".+?/film/(.+?)/", RegexOptions.Compiled);
        private static readonly Regex movieNotFoundExpr = new Regex("� ���������, �� ������ ������� ������ �� �������", RegexOptions.Compiled);

        private static readonly Regex movieInfoExpr =
            new Regex(
                @"<h1 class=""moviename-big"".+?>(.+?)</h1>.+?<span itemprop=""alternativeHeadline"">(.+?)</span>.+?<td class=""type"">���</td>.+?>(\d+)</a>.+?<td class=""type"">������</td>.*?<td.*?>(.+?)</td>.+?<td class=""type"">��������</td>.*?<td.*?>(.+?)</td>.+?<span class=""rating_ball"">(.+?)</span>.+?IMDb: ([\d.]+)",
                RegexOptions.Singleline | RegexOptions.Compiled);

        public static int GetMovieId(string title)
        {
            var searchResultPage = WebPageParser.GetPage(KinopoiskUri, searchRequestPrefix + title);
            List<string> parseResult;

            if (WebPageParser.TryParsePage(searchResultPage, movieNotFoundExpr, out parseResult))
                throw new ArgumentException("����� �� ������");

            if (!WebPageParser.TryParsePage(searchResultPage, movieIdExpr, out parseResult))
                throw new Exception("������ ��� ��������� id ������");

            return int.Parse(parseResult[0]);
        }

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            List<string> parseResult;
            var filmHref = $"/film/{GetMovieId(searchTitle)}/";
            var filmPage = WebPageParser.GetPage(KinopoiskUri, filmHref);

            if (!WebPageParser.TryParsePage(filmPage, movieInfoExpr, out parseResult))
                throw new Exception("������ ��� ��������� ���������� � ������");
          
            var rating = new Dictionary<string, string>
            {
                ["���������"] = parseResult[5],
                ["IMDb"] = parseResult[6]
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

        public List<MovieInfo> GetWeekTopMovies()
        {
            throw new NotImplementedException();
        }

        public List<MovieInfo> GetWeekNewMovies()
        {
            throw new NotImplementedException();
        }
    }
}