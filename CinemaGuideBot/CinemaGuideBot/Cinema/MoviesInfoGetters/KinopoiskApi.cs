using System;
using System.Linq;
using System.Threading;
using CinemaGuideBot.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CinemaGuideBot.Cinema.MovieInfoParsers;

namespace CinemaGuideBot.Cinema.MoviesInfoGetters
{
    public class KinopoiskApi : IMoviesInfoGetter
    {
        public static readonly Uri KinopoiskApiUri = new Uri("https://getmovie.cc");

        private const string token = "037313259a17be837be3bd04a51bf678";
        private const int millisecondsDelay = 2000;
        private readonly IMovieInfoParser movieInfoParser;

        public KinopoiskApi(IMovieInfoParser movieInfoParser)
        {
            this.movieInfoParser = movieInfoParser;
        }

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            return GetMovieInfoAsync(searchTitle).Result;
        }

        public MovieInfo GetMovieInfo(int movieId)
        {
            return GetMovieInfoAsync(movieId).Result;
        }

        public async Task<MovieInfo> GetMovieInfoAsync(string searchTitle)
        {
            var movieSearchPage = await KinopoiskWebPage.GetMovieSearchPageAsync(searchTitle);
            var movieId = KinopoiskWebPage.GetMoviesIds(movieSearchPage)[0];
            return await GetMovieInfoAsync(movieId);
        }

        public async Task<MovieInfo> GetMovieInfoAsync(int movieId)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var pageTask = WebPageParser.GetPageAsync(KinopoiskApiUri, $"/api/kinopoisk.json?id={movieId}&token={token}");
                var completedTask = await Task.WhenAny(pageTask, Task.Delay(millisecondsDelay, cancellationTokenSource.Token));

                if (pageTask != completedTask)
                    return new MovieInfo();

                cancellationTokenSource.Cancel();
                var page = Regex.Unescape(pageTask.Result);
                return movieInfoParser.Parse(page);
            }
        }

        public List<MovieInfo> GetWeekTopMovies()
        {
            var cashBlock = KinopoiskWebPage.GetCashBlock();
            var moviesIds = KinopoiskWebPage.GetMoviesIds(cashBlock);
            var tasks = moviesIds.Select(GetMovieInfoAsync).ToArray();
            Task.WaitAll(tasks);
            return tasks.Select(t => t.Result).ToList();
        }

        public List<MovieInfo> GetWeekNewMovies()
        {
            var weekPremieresBlock = KinopoiskWebPage.GetWeekPremieresBlock();
            var moviesIds = KinopoiskWebPage.GetMoviesIds(weekPremieresBlock);
            var tasks = moviesIds.Select(GetMovieInfoAsync).ToArray();
            Task.WaitAll(tasks);
            return tasks.Select(t => t.Result).ToList();
        }
    }

    class ApiSearchResult : IMovieInfoJsonObject
    {
        public string Name_ru { get; set; }
        public string Name_en { get; set; }
        public string Year { get; set; }
        public string Country { get; set; }
        public Dictionary<string, Person[]> Creators { get; set; }
        public ApiRating Rating { get; set; }

        public MovieInfo ToMovieInfo()
        {
            var rating = new Dictionary<string, string>
            {
                ["Кинопоиск"] = Rating.Kp_rating,
                ["IMDb"] = Rating.Imdb
            };

            return new MovieInfo
            {
                Title = Name_ru,
                OriginalTitle = Name_en,
                Year = string.IsNullOrEmpty(Year) ? MovieInfo.DefaultYear : int.Parse(Year),
                Country = Country,
                Director = string.Join(", ", Creators["director"].Select(d => d.Name_person_ru)),
                Rating = rating
            };
        }
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