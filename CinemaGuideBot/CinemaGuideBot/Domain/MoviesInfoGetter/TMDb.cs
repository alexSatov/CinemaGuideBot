using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using System.Threading;

namespace CinemaGuideBot.Domain.MoviesInfoGetter
{
    public class TMDb : IMoviesInfoGetter
    {
        private readonly ServiceClient serviceClient;
        private const string apiToken = "14fc21ab59267fc7b0990d27ab14d6fb";

        public TMDb()
        {
            serviceClient = new ServiceClient(apiToken);
        }

        public MovieInfo GetMovieInfo(string searchTitle)
        {
            var result = serviceClient.Movies.SearchAsync(searchTitle, "RU", true, null, true, 1, CancellationToken.None);
            var mostPopularMovie = result.Result.Results.OrderByDescending(x => x.Popularity).FirstOrDefault();

            if (mostPopularMovie == null)
                throw new ArgumentException("can't find movie by this title");

            var movieInfoResult = serviceClient.Movies.GetAsync(mostPopularMovie.Id, "RU", true, CancellationToken.None);
            var movieInfo = movieInfoResult.Result;

            var rating = new Dictionary<string, string> {{"Tmdb", movieInfo.VoteCount.ToString()}};
            var director = movieInfo
                .Credits
                .Crew
                .First(x => x.Job.Equals("director", StringComparison.InvariantCultureIgnoreCase))
                ?.Name;

            return new MovieInfo
            {
                Country = movieInfo.Countries.First()?.Name,
                Title = movieInfo.Title,
                Director = director,
                Rating = rating,
                Year = movieInfo.ReleaseDate.GetValueOrDefault().Year,
                OriginalTitle = movieInfo.OriginalTitle
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
