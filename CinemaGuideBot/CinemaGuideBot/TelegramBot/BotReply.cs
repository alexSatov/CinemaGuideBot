using System;
using System.Linq;
using CinemaGuideBot.Cinema;
using System.Collections.Generic;
using CinemaGuideBot.TelegramBot.Localisation;

namespace CinemaGuideBot.TelegramBot
{
    public class BotReply : IPhraseDict
    {
        public const string DefaultLanguage = "RU";
        public HashSet<string> SupportedLanguages { get; }

        private IPhraseDict currentPhraseDict;

        private static readonly Dictionary<string, IPhraseDict> phraseDicts = new Dictionary<string, IPhraseDict>
        {
            ["RU"] = new RuPhraseDict(),
            ["EN"] = new EnPhraseDict()
        };

        public BotReply()
        {
            SupportedLanguages = new HashSet<string>(phraseDicts.Keys);
            SetLanguage();
        }

        public void SetLanguage(string language = DefaultLanguage)
        {
            if (SupportedLanguages.Contains(language.ToUpper()))
                currentPhraseDict = phraseDicts[language];
            else
                throw new ArgumentException($"Unsupported language \"{language}\"");
        }

        public string ReplyFrom(MovieInfo movieInfo)
        {
            if (string.IsNullOrEmpty(movieInfo.Title))
                return "";

            var year = movieInfo.Year == MovieInfo.DefaultYear ? null : $"{Year}: {movieInfo.Year}";
            var director = string.IsNullOrEmpty(movieInfo.Director) ? null : $"{Director}: {movieInfo.Director}";
            var country = string.IsNullOrEmpty(movieInfo.Country) ? null : $"{Country}: {movieInfo.Country}";

            var title = movieInfo.Title == movieInfo.OriginalTitle || movieInfo.OriginalTitle == null
                    ? $"{Title}: {movieInfo.Title}"
                    : $"{Title}: {movieInfo.Title} ({movieInfo.OriginalTitle})";

            var rating = string.Join(", ", movieInfo.Rating
                .Where(r => !string.IsNullOrEmpty(r.Value) && !r.Value.StartsWith("0"))
                .Select(r => $"{r.Key}: {r.Value}"));

            return string.Join("\r\n",
                new string[] { title, year, director, country, rating }
                .Where(p => !string.IsNullOrEmpty(p))) + "\r\n";
        }


        public string Greeting => currentPhraseDict.Greeting;

        public string StartCommandDescription => currentPhraseDict.StartCommandDescription;
        public string HelpCommandDescription => currentPhraseDict.HelpCommandDescription;
        public string MovieSearchCommandDescription => currentPhraseDict.MovieSearchCommandDescription;
        public string WeekPremieresCommandDescription => currentPhraseDict.WeekPremieresCommandDescription;
        public string WeekTopCommandDescription => currentPhraseDict.WeekTopCommandDescription;

        public string HelpText => currentPhraseDict.HelpText;
        public string EnterMovieTitle => currentPhraseDict.EnterMovieTitle;
        public string MovieNotFound => currentPhraseDict.MovieNotFound;
        public string UnknownCommand => currentPhraseDict.UnknownCommand;
        public string UnexpectedError => currentPhraseDict.UnexpectedError;

        public string Title => currentPhraseDict.Title;
        public string Year => currentPhraseDict.Year;
        public string Country => currentPhraseDict.Country;
        public string Director => currentPhraseDict.Director;
    }
}