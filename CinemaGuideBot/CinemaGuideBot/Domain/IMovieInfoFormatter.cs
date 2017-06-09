namespace CinemaGuideBot.Domain
{
    public interface IMovieInfoFormatter
    {
        string Format(MovieInfo movieInfo);
    }
}