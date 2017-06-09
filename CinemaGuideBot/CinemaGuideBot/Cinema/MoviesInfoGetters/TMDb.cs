using System;
using System.Linq;
using TMDbLib.Client;
using System.Globalization;
using TMDbLib.Objects.Movies;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CinemaGuideBot.Cinema.MoviesInfoGetters
{
    public class TMDb : IMoviesInfoGetter
    {
        public readonly TMDbClient Client;

        private const string token = "14fc21ab59267fc7b0990d27ab14d6fb";

        public TMDb()
        {
            Client = new TMDbClient(token) { DefaultLanguage = "RU" };
        }

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            return GetMovieInfoAsync(searchTitle).Result;
        }

        public MovieInfo GetMovieInfo(int movieId)
        {
            return GetMovieInfoAsync(movieId).Result;
        }

        private async Task<MovieInfo> GetMovieInfoAsync(string searchTitle)
        {
            var movieSearchResult = await Client.SearchMovieAsync(searchTitle);

            var mostPopularMovie = movieSearchResult
                .Results
                .OrderByDescending(x => x.Popularity)
                .FirstOrDefault();

            if (mostPopularMovie == null)
                throw new ArgumentException("Movie not found");

            return GetMovieInfo(mostPopularMovie.Id);
        }

        private async Task<MovieInfo> GetMovieInfoAsync(int movieId)
        {
            var movieInfo = await Client.GetMovieAsync(movieId, MovieMethods.Credits | MovieMethods.Undefined | MovieMethods.ReleaseDates);
            var rating = new Dictionary<string, string> { { "Tmdb", movieInfo.VoteAverage.ToString(CultureInfo.InvariantCulture) } };

            var director = movieInfo
                .Credits?
                .Crew
                .FirstOrDefault(x => x.Job.Equals("director", StringComparison.InvariantCultureIgnoreCase))?
                .Name;

            return new MovieInfo
            {
                Title = movieInfo.Title,
                OriginalTitle = movieInfo.OriginalTitle,
                Year = movieInfo.ReleaseDate?.Year ?? MovieInfo.DefaultYear,
                Rating = rating,
                Director = director,
                Country = string.Join(", ", movieInfo.ProductionCountries.Select(pc => pc.Name)),
            };
        }

        public List<MovieInfo> GetWeekTopMovies()
        {
            var moviesSearchResult = Client.GetMovieNowPlayingListAsync(Client.DefaultLanguage).Result.Results;
            var tasks = moviesSearchResult.Select(m => GetMovieInfoAsync(m.Id)).ToArray();
            Task.WaitAll(tasks);
            return tasks
                .Select(t => t.Result)
                .OrderByDescending(movie => movie.Rating["Tmdb"])
                .Take(5)
                .ToList();
        }

        public List<MovieInfo> GetWeekNewMovies()
        {
            var today = DateTime.Today;
            var startOfCurrentWeek = today.AddDays(-(int)today.DayOfWeek);
            var moviesSearchResult = Client.GetMovieNowPlayingListAsync(Client.DefaultLanguage).Result.Results;

            var tasks = moviesSearchResult
                .Where(m => m.ReleaseDate != null && m.ReleaseDate >= startOfCurrentWeek)
                .Select(m => GetMovieInfoAsync(m.Id))
                .ToArray();

            Task.WaitAll(tasks);
            return tasks.Select(t => t.Result).ToList();
        }
    }
}
