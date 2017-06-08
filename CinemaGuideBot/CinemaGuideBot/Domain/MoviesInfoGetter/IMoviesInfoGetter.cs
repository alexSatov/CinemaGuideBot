using System.Collections.Generic;

namespace CinemaGuideBot.Domain.MoviesInfoGetter
{
    public interface IMoviesInfoGetter
    {
        MovieInfo GetMovieInfo(string searchTitle);
        List<MovieInfo> GetWeekTopMovies();
        List<MovieInfo> GetWeekNewMovies();
    }
}