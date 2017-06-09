using System.Linq;

namespace CinemaGuideBot.Domain.MovieInfoFormatters
{
    public class SimpleMovieInfoFormatter : IMovieInfoFormatter
    {
        public string Format(MovieInfo movieInfo)
        {
            if (string.IsNullOrEmpty(movieInfo.Title))
                return "";

            var year = movieInfo.Year == MovieInfo.DefaultYear ? null : $"Год: {movieInfo.Year}";
            var director = string.IsNullOrEmpty(movieInfo.Director) ? null : $"Режиссер: {movieInfo.Director}";
            var country = string.IsNullOrEmpty(movieInfo.Country) ? null : $"Страна: {movieInfo.Country}";

            var title = movieInfo.Title == movieInfo.OriginalTitle || movieInfo.OriginalTitle == null
                    ? $"Название: {movieInfo.Title}"
                    : $"Название: {movieInfo.Title} ({movieInfo.OriginalTitle})";

            var rating = string.Join(", ", movieInfo.Rating
                .Where(r => !string.IsNullOrEmpty(r.Value) && !r.Value.StartsWith("0"))
                .Select(r => $"{r.Key}: {r.Value}"));

            return string.Join("\r\n",
                new string[] { title, year, director, country, rating }
                .Where(p => !string.IsNullOrEmpty(p))) + "\r\n";
        }
    }
}