using System.Collections.Generic;

namespace CinemaGuideBot.Cinema.MoviesInfoGetters
{
    public interface IMoviesInfoGetter
    {
        MovieInfo GetMovieInfo(string searchTitle);
        List<MovieInfo> GetWeekTopMovies();
        List<MovieInfo> GetWeekNewMovies();
    }
}