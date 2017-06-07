using System;
using System.Text.RegularExpressions;

namespace CinemaGuideBot.Domain
{
    public class KinopoiskWebPageMIG : IMovieInfoGetter
    {
        public static readonly Uri KinopoiskUri = new Uri("https://www.kinopoisk.ru/");
        private static readonly Regex filmHref = new Regex("<a href=\".+?/film/.+?\".+?data-url=\"(.+?)\".+?</a>");

        public MovieInfo[] GetMovieInfo(string title)
        {
            throw new NotImplementedException();
        }
    }
}
