using System.Collections.Generic;

namespace CinemaGuideBot.Domain.MoviesInfoGetters
{
    public interface IMoviesInfoGetter
    {
        MovieInfo GetMovieInfo(string searchTitle);
        List<MovieInfo> GetWeekTopMovies();
        List<MovieInfo> GetWeekNewMovies();
    }
}