using System;
using System.Collections.Generic;
using CinemaGuideBot.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace CinemaGuideBot.Tests
{
    [TestFixture]
    public class MovieInfoShould
    {
        [SetUp]
        public void SetUp()
        {
            correctMovieInfo = new MovieInfo
            {
                Country = "Russia",
                Director = "Mr. mediocrity",
                OriginalTitle = "Very expensive bad film",
                Rating = new Dictionary<string, string> {{"imdb", "0"}},
                Title = "THE BEST FILM EVER",
                Year = DateTime.Now.Year
            };
        }

        private MovieInfo correctMovieInfo;

        [Test]
        public void ConvertInEmptyString_WhenAllFieldsEmpty()
        {
            new MovieInfo().ToString().Should().BeEmpty();
        }

        [Test]
        public void CorrectlyConvertName_WhenAllTitlesExist()
        {
            var movieInfo = new MovieInfo
            {
                Title = "TEST",
                OriginalTitle = "OK",
                Rating = new Dictionary<string, string>(),
                Year = 1800
            };
            movieInfo.ToString().Should().Be("Название: TEST (OK)\r\n");
        }

        [Test]
        public void SetYear_WhenInCorrectRange()
        {
            var movieInfo = new MovieInfo {Year = DateTime.Now.Year};
            movieInfo.Year.Should().Be(DateTime.Now.Year);
        }

        [Test]
        public void ThrowEsception_WhenYearIsNegative()
        {
            Action movieInfoInit = () => new MovieInfo {Year = -1};
            movieInfoInit.ShouldThrow<ArgumentException>().WithMessage("Incorrect year (-1)");
        }

        [Test]
        public void ToString_WhenAllFieldsNotEmpty()
        {
            var correctString =
                $"Название: THE BEST FILM EVER (Very expensive bad film)\r\nГод: {DateTime.Today.Year}\r\n" +
                "Режиссер: Mr. mediocrity\r\nСтрана: Russia\r\nimdb: 0\r\n";
            correctMovieInfo.ToString().Should().Be(correctString);
        }

        [Test]
        public void ToStringWithOnlyTitle_WhenOriginDoesntExist()
        {
            var movieInfo = new MovieInfo {Title = "TEST", Rating = new Dictionary<string, string>(), Year = 1800};
            movieInfo.ToString().Should().Be("Название: TEST\r\n");
        }
    }
}