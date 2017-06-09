namespace CinemaGuideBot.Cinema.MovieInfoParsers
{
    public interface IMovieInfoParser
    {
        MovieInfo Parse(string source);
    }
}