using System;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using CinemaGuideBot.Infrastructure;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Domain.MoviesInfoGetters
{
    public class KinopoiskApi : IMoviesInfoGetter
    {
        public static readonly Uri KinopoiskApiUri = new Uri("https://getmovie.cc");

        private const string token = "037313259a17be837be3bd04a51bf678";
        private const int millisecondsDelay = 2000;

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            return GetMovieInfoAsync(searchTitle).Result;
        }

        public MovieInfo GetMovieInfo(int movieId)
        {
            return GetMovieInfoAsync(movieId).Result;
        }

        private static async Task<MovieInfo> GetMovieInfoAsync(string searchTitle)
        {
            var movieSearchPage = await KinopoiskWebPage.GetMovieSearchPageAsync(searchTitle);
            var movieId = KinopoiskWebPage.GetMoviesId(movieSearchPage)[0];
            return await GetMovieInfoAsync(movieId);
        }

        private static async Task<MovieInfo> GetMovieInfoAsync(int movieId)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var pageTask = WebPageParser.GetPageAsync(KinopoiskApiUri, $"/api/kinopoisk.json?id={movieId}&token={token}");
                var completedTask = await Task.WhenAny(pageTask, Task.Delay(millisecondsDelay, cancellationTokenSource.Token));

                if (pageTask != completedTask)
                    return new MovieInfo();

                cancellationTokenSource.Cancel();
                var page = Regex.Unescape(pageTask.Result);
                var searchResult = JsonConvert.DeserializeObject<ApiSearchResult>(page);
                
                var rating = new Dictionary<string, string>
                {
                    ["Кинопоиск"] = searchResult.Rating.Kp_rating,
                    ["IMDb"] = searchResult.Rating.Imdb
                };

                return new MovieInfo
                {
                    Title = searchResult.Name_ru,
                    OriginalTitle = searchResult.Name_en,
                    Year = string.IsNullOrEmpty(searchResult.Year) ? MovieInfo.DefaultYear : int.Parse(searchResult.Year),
                    Country = searchResult.Country,
                    Director = searchResult.Creators["director"][0].Name_person_ru,
                    Rating = rating
                };
            }
        }

        public List<MovieInfo> GetWeekTopMovies()
        {
            var cashBlockElement = KinopoiskWebPage.GetCashBlock();
            var moviesId = KinopoiskWebPage.GetMoviesId(cashBlockElement);
            var tasks = moviesId.Select(GetMovieInfoAsync).ToArray();
            Task.WaitAll(tasks);
            return tasks.Select(t => t.Result).ToList();
        }

        public List<MovieInfo> GetWeekNewMovies()
        {
            var weekPremieresElement = KinopoiskWebPage.GetWeekPremieresBlock();
            var moviesId = KinopoiskWebPage.GetMoviesId(weekPremieresElement);
            var tasks = moviesId.Select(GetMovieInfoAsync).ToArray();
            Task.WaitAll(tasks);
            return tasks.Select(t => t.Result).ToList();
        }
    }

    class ApiSearchResult
    {
        public string Name_ru { get; set; }
        public string Name_en { get; set; }
        public string Year { get; set; }
        public string Country { get; set; }
        public Dictionary<string, Person[]> Creators { get; set; }
        public ApiRating Rating { get; set; }
    }

    class ApiRating
    {
        public string Imdb { get; set; }
        public string Kp_rating { get; set; }
    }

    class Person
    {
        public string Name_person_ru { get; set; }
    }
}