namespace CinemaGuideBot.Domain.MovieInfoFormatters
{
    public interface IMovieInfoFormatter
    {
        string Format(MovieInfo movieInfo);
    }
}