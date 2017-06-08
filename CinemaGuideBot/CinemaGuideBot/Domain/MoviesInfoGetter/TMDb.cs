using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace CinemaGuideBot.Domain.MoviesInfoGetter
{
    public class TMDb : IMoviesInfoGetter
    {
        public readonly TMDbClient Client;
        private const string apiToken = "14fc21ab59267fc7b0990d27ab14d6fb";

        public TMDb()
        {
            Client = new TMDbClient(apiToken);
            Client.DefaultLanguage = "RU";
        }

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            var result = Client.SearchMovieAsync(searchTitle);
            var mostPopularMovie = result.Result.Results.OrderByDescending(x => x.Popularity).FirstOrDefault();

            if (mostPopularMovie == null)
                throw new ArgumentException("can't find movie by this title");

            return GetMovieInfo(mostPopularMovie.Id);
        }

        public MovieInfo GetMovieInfo(int movieId)
        {
            var movieInfoResult = Client.GetMovieAsync(movieId, MovieMethods.Credits| MovieMethods.Undefined | MovieMethods.ReleaseDates);
            var movieInfo = movieInfoResult.Result;
            var rating = new Dictionary<string, string> { { "Tmdb", movieInfo.VoteAverage.ToString(CultureInfo.InvariantCulture) } };
            var director = movieInfo
                .Credits?
                .Crew
                .FirstOrDefault(x => x.Job.Equals("director", StringComparison.InvariantCultureIgnoreCase))
                ?.Name;

            return new MovieInfo
            {
                Country = movieInfo.ProductionCountries.Any()?movieInfo.ProductionCountries.First().Name : null,
                Title = movieInfo.Title,
                Director = director,
                Rating = rating,
                Year = movieInfo.ReleaseDate?.Year ?? 0,
                OriginalTitle = movieInfo.OriginalTitle
            };
        }

        public List<MovieInfo> GetWeekTopMovies()
        {
            var searchTask = Client.GetMovieNowPlayingListAsync("RU");
            return searchTask.Result.Results
                .AsParallel()
                .Select(movie => GetMovieInfo(movie.Id))
                .OrderByDescending(movie => movie.Rating["Tmdb"])
                .Take(5)
                .ToList();
        }

        public List<MovieInfo> GetWeekNewMovies()
        {
            var today = DateTime.Today;
            var startOfCurrentWeek = today.AddDays(-(int)today.DayOfWeek);
            var searchTask = Client.GetMovieNowPlayingListAsync();
            return searchTask.Result
                .Results
                .Where(movie => movie.ReleaseDate != null && movie.ReleaseDate >= startOfCurrentWeek)
                .AsParallel()
                .Select(movie => GetMovieInfo(movie.Id))
                .ToList();
        }
    }
}
