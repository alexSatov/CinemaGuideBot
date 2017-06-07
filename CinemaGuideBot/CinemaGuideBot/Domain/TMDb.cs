using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using System.Threading;

namespace CinemaGuideBot.Domain
{
    public class TMDb : IMovieInfoGetter
    {
        private readonly ServiceClient serviceClient;
        private readonly string apiToken;

        public TMDb(string apiToken)
        {
            this.apiToken = apiToken;
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
            
            var rating = new Dictionary<string, double>();
            rating.Add("Tmdb", movieInfo.VoteCount);
            return new MovieInfo
            {
                Country = movieInfo.Countries.First()?.Name,
                Title = movieInfo.Title,
                Director = movieInfo.Credits.Crew.First(x => x.Job.Equals("director", StringComparison.InvariantCultureIgnoreCase))?.Name,
                Rating = rating,
                Year = movieInfo.ReleaseDate.GetValueOrDefault().Year,
                OriginalTitle = movieInfo.OriginalTitle
            };
        }

    }
}
