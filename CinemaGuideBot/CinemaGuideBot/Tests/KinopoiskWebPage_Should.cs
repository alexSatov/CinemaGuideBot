using System;
using System.Threading.Tasks;
using CinemaGuideBot.Cinema.MoviesInfoGetters;
using FluentAssertions;
using NUnit.Framework;

namespace CinemaGuideBot.Tests
{
    [TestFixture]
    public class KinopoiskWebPage_Should
    {
        [TestCase("Matrix")]
        [TestCase("Alien")]
        [TestCase("Cloud Atlas")]
        public void GetHtmlPageWithSearchResult_WhenSearchRealMovie(string title)
        {
            var page = KinopoiskWebPage.GetMovieSearchPageAsync(title).Result;
            page.StartsWith("<!DOCTYPE html").Should().BeTrue();
        }

        [TestCase("12434")]
        [TestCase("sda")]
        [TestCase("-(*")]
        public void ThrowException_WhenSearchFakeMovie(string title)
        {
            new Action(() => Task.WaitAll(KinopoiskWebPage.GetMovieSearchPageAsync(title)))
                .ShouldThrow<ArgumentException>().WithMessage("Movie not found");
        }

        [TestCase("Matrix")]
        [TestCase("Alien")]
        [TestCase("Cloud Atlas")]
        public void GetMoviesIds_WhenParsingCorrectPage(string title)
        {
            var page = KinopoiskWebPage.GetMovieSearchPageAsync(title).Result;
            var moviesIds = KinopoiskWebPage.GetMoviesIds(page);
            moviesIds.Should().NotBeEmpty();
        }

        [TestCase("class=\"block block_cash\"", "GetCashBlock")]
        [TestCase("class=\"prem_list\"", "GetWeekPremieresBlock")]
        public void GetCorrectHtmlBlock_WhenRequestIt(string htmlClass, string methodName)
        {
            var pageBlock = (string) typeof(KinopoiskWebPage).GetMethod(methodName).Invoke(null, new object[] {});
            pageBlock.Contains(htmlClass).Should().BeTrue();
        }
    }
}