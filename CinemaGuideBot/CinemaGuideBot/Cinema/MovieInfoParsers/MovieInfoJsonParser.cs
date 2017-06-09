using Newtonsoft.Json;

namespace CinemaGuideBot.Cinema.MovieInfoParsers
{
    public interface IMovieInfoJsonObject
    {
        MovieInfo ToMovieInfo();
    }

    public class MovieInfoJsonParser<T> : IMovieInfoParser
        where T : IMovieInfoJsonObject
    {
        public MovieInfo Parse(string source)
        {
            var movieInfoJsonObject = JsonConvert.DeserializeObject<T>(source);
            return movieInfoJsonObject.ToMovieInfo();
        }
    }
}