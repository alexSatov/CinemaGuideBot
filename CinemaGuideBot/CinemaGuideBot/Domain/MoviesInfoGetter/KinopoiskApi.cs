using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CinemaGuideBot.Domain.MovieInfoGetter;
using CinemaGuideBot.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CinemaGuideBot.Domain.MoviesInfoGetter
{
    public class KinopoiskApi : IMoviesInfoGetter
    {
        public static readonly Uri KinopoiskApiUri = new Uri("https://getmovie.cc");
        private const string token = "037313259a17be837be3bd04a51bf678";

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            var movieId = KinopoiskWebPage.GetMovieId(searchTitle);
            var page = WebPageParser.GetPage(KinopoiskApiUri, $"/api/kinopoisk.json?id={movieId}&token={token}");
            page = Regex.Unescape(page);
            var fullMovieInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(page);

            var director = ((fullMovieInfo["creators"] as JObject)
                    .GetValue("director")
                    .First as JObject)
                .GetValue("name_person_ru");

            var jRating = (JObject) fullMovieInfo["rating"];
            var rating = new Dictionary<string, string>
            {
                ["���������"] = jRating.GetValue("imdb").ToString(),
                ["IMDb"] = jRating.GetValue("kp_rating").ToString()
            };

            return new MovieInfo
            {
                Title = fullMovieInfo["name_ru"].ToString(),
                OriginalTitle = fullMovieInfo["name_en"]?.ToString(),
                Year = int.Parse(fullMovieInfo["year"].ToString()),
                Country = fullMovieInfo["country"]?.ToString(),
                Director = director.ToString(),
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