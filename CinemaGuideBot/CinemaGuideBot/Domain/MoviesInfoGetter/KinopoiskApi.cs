using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CinemaGuideBot.Infrastructure;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Domain.MoviesInfoGetter
{
    public class KinopoiskApi : IMoviesInfoGetter
    {
        public static readonly Uri KinopoiskApiUri = new Uri("https://getmovie.cc");
        private const string token = "037313259a17be837be3bd04a51bf678";

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
            var page = await WebPageParser.GetPageAsync(KinopoiskApiUri, $"/api/kinopoisk.json?id={movieId}&token={token}");
            page = Regex.Unescape(page);
            var fullMovieInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(page);

            var director = ((fullMovieInfo["creators"] as JObject)
                    .GetValue("director")
                    .First as JObject)
                .GetValue("name_person_ru");

            var jRating = (JObject)fullMovieInfo["rating"];
            var rating = new Dictionary<string, string>
            {
                ["Кинопоиск"] = jRating.GetValue("imdb").ToString(),
                ["IMDb"] = jRating.GetValue("kp_rating").ToString()
            };

            return new MovieInfo
            {
                Title = fullMovieInfo["name_ru"]?.ToString(),
                OriginalTitle = fullMovieInfo["name_en"]?.ToString(),
                Year = fullMovieInfo["year"] == null ? 1800 : int.Parse(fullMovieInfo["year"].ToString()),
                Country = fullMovieInfo["country"]?.ToString(),
                Director = director.ToString(),
                Rating = rating
            };
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
            throw new NotImplementedException();
        }
    }
}