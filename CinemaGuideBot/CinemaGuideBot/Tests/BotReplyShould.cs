using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CinemaGuideBot.Cinema;
using CinemaGuideBot.TelegramBot;
using CinemaGuideBot.TelegramBot.Localisation;
using FluentAssertions;
using NUnit.Framework;

namespace CinemaGuideBot.Tests
{
    [TestFixture]
    internal class BotReplyShould
    {
        [SetUp]
        public void SetUp()
        {
            correctMovieInfo = new MovieInfo
            {
                Country = "Russia",
                Director = "Mr. mediocrity",
                OriginalTitle = "Very expensive bad film",
                Rating = new Dictionary<string, string> { { "imdb", "1" } },
                Title = "THE BEST FILM EVER",
                Year = DateTime.Now.Year
            };

            phraseDicts = new IPhraseDict[] {new RuPhraseDict(), new EnPhraseDict()};
            testReply = new BotReply(phraseDicts);
        }

        private MovieInfo correctMovieInfo;
        private IPhraseDict[] phraseDicts;
        private BotReply testReply;

        [Test]
        public void HaveAllUniqueLanguages_AfterCreation()
        {
            var expectedLanguages = phraseDicts.Select(l => l.Language);
            expectedLanguages.All(testReply.SupportedLanguages.Contains).Should().BeTrue();
            testReply.SupportedLanguages.Count.Should().Be(2);
        }

        [Test]
        public void HaveInitCurrentPhraseDict_AfterCreation()
        {
            testReply.CurrentPhraseDict.Should().NotBeNull();
        }

        [Test]
        public void NotSetLanguage_WhenHeUnsupported()
        {
            var setAction = new Action(() => testReply.SetLanguage("FakeLanguage"));
            setAction.ShouldThrow<ArgumentException>().WithMessage("Unsupported language \"FakeLanguage\"");
        }

        [Test]
        public void SetLanguage_WhenHeSupported()
        {
            var newLang = phraseDicts.Select(x => x.Language)
                .First(x => x != testReply.CurrentPhraseDict.Language && testReply.SupportedLanguages.Contains(x));
            testReply.SetLanguage(newLang);
            testReply.CurrentPhraseDict.Language.Should().BeEquivalentTo(newLang);
        }

        [Test]
        public void GetEmptyReply_WhenMovieInfoEmpty()
        {
            testReply.ReplyFrom(new MovieInfo()).ShouldAllBeEquivalentTo(string.Empty);
        }

        [Test]
        public void CorrectReplyFromCorrectMovieInfo()
        {
            var correctString =
                $"Название: THE BEST FILM EVER (Very expensive bad film)\r\nГод: {DateTime.Today.Year}\r\n" +
                "Режиссер: Mr. mediocrity\r\nСтрана: Russia\r\nimdb: 1\r\n";
            testReply.ReplyFrom(correctMovieInfo).Should().Be(correctString);
        }

        [Test]
        public void CorrectReplyWithOnlyTitle_WhenOriginDoesntExist()
        {
            var movieInfo = new MovieInfo { Title = "TEST", Rating = new Dictionary<string, string>(), Year = 1800 };
            testReply.ReplyFrom(movieInfo).Should().Be("Название: TEST\r\n");
        }

        [Test]
        public void CorrectReplyMovieInfo_WhenOfTheRatingsIsZero()
        {
            var movieInfo = new MovieInfo { Title = "TEST", Rating = new Dictionary<string, string>(){{ "imdb", "1" }, { "fake", "0" } }, Year = 1800 };
            testReply.ReplyFrom(movieInfo).Should().Be("Название: TEST\r\nimdb: 1\r\n");
        }

        [Test]
        public void NotReplyMovieInfo_WhenTitleEmpty()
        {
            var movieInfo = new MovieInfo {Rating = new Dictionary<string, string>() { { "imdb", "1" }, { "fake", "0" } }, Year = 1800 };
            testReply.ReplyFrom(movieInfo).Should().Be(string.Empty);
        }
    }
}