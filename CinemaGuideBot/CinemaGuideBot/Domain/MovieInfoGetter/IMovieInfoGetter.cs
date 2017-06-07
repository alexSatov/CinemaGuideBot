namespace CinemaGuideBot.Domain.MovieInfoGetter
{
    public interface IMovieInfoGetter
    {
        MovieInfo GetMovieInfo(string searchTitle);
    }
}