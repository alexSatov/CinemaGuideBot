using System.Linq;
using CinemaGuideBot.TelegramBot;
using CinemaGuideBot.TelegramBot.Localisation;
using FluentAssertions;
using NUnit.Framework;

namespace CinemaGuideBot.Tests
{
    [TestFixture]
    class BotReplyShould
    {
        private IPhraseDict[] phraseDicts;
        [SetUp]
        public void SetUp()
        {
            phraseDicts = new IPhraseDict[] { new RuPhraseDict(), new EnPhraseDict() };
        }

        [Test]
        public void HaveAllUniqueLanguages_AfterCreation()
        { 
            var newReply = new BotReply(phraseDicts);
            var expectedLanguages = phraseDicts.Select(l => l.Language);
            expectedLanguages.All(newReply.SupportedLanguages.Contains).Should().BeTrue();
            newReply.SupportedLanguages.Count.Should().Be(2);
        }
    }
}
