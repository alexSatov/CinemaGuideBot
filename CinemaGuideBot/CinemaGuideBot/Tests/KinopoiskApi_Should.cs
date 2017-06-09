using NUnit.Framework;
using FluentAssertions;
using CinemaGuideBot.Cinema;
using System.Collections.Generic;
using CinemaGuideBot.Cinema.MovieInfoParsers;
using CinemaGuideBot.Cinema.MoviesInfoGetters;

namespace CinemaGuideBot.Tests
{
    [TestFixture]
    public class KinopoiskApi_Should
    {
        private const int movieId = 464484;
        private const string movieTitle = "Облачный атлас";
        private readonly KinopoiskApi api = new KinopoiskApi(new MovieInfoJsonParser<ApiSearchResult>());

        [Test]
        public void GetCorrectMovieInfo_WhenSearchRealMovie()
        {
            api.GetMovieInfo(movieTitle).ShouldBeEquivalentTo(new MovieInfo
            {
                Title = movieTitle,
                OriginalTitle = "Cloud Atlas",
                Year = 2012,
                Director = "Лана Вачовски, Том Тыквер, Лилли Вачовски",
                Country = "США, Германия, Гонконг, Сингапур",
                Rating = new Dictionary<string, string> { ["Кинопоиск"] = "7.726", ["IMDb"] = "7.50" }
            });
        }

        [Test]
        public void GetSameMovieInfos_WhenSearchByTitleAndId()
        {
            var titleSearchResult = api.GetMovieInfo(movieTitle);
            var idSearchResult = api.GetMovieInfo(movieId);
            titleSearchResult.ShouldBeEquivalentTo(idSearchResult);
        }
    }
}