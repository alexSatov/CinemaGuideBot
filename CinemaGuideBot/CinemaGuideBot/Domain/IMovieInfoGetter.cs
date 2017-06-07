namespace CinemaGuideBot.Domain
{
    public interface IMovieInfoGetter
    {
        MovieInfo GetMovieInfo(string searchTitle);
    }
}