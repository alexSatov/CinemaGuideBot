using System;
using System.Linq;
using CinemaGuideBot.Cinema;
using System.Collections.Generic;
using CinemaGuideBot.TelegramBot.Localisation;

namespace CinemaGuideBot.TelegramBot
{
    public class BotReply
    {
        public const string DefaultLanguage = "RU";
        public IPhraseDict CurrentPhraseDict { get; private set; }
        public static HashSet<string> SupportedLanguages { get; private set; }

        private readonly Dictionary<string, IPhraseDict> phraseDicts;
        
        public BotReply(IPhraseDict[] phraseDicts)
        {
            this.phraseDicts = phraseDicts.ToDictionary(phraseDict => phraseDict.Language);
            SupportedLanguages = new HashSet<string>(this.phraseDicts.Keys);
            SetLanguage();
        }

        public void SetLanguage(string language = DefaultLanguage)
        {
            if (SupportedLanguages.Contains(language.ToUpper()))
                CurrentPhraseDict = phraseDicts[language.ToUpper()];
            else
                throw new ArgumentException($"Unsupported language \"{language}\"");
        }

        public string ReplyFrom(MovieInfo movieInfo)
        {
            if (string.IsNullOrEmpty(movieInfo.Title))
                return "";

            var year = movieInfo.Year == MovieInfo.DefaultYear ? null : $"{CurrentPhraseDict.Year}: {movieInfo.Year}";
            var director = string.IsNullOrEmpty(movieInfo.Director) ? null : $"{CurrentPhraseDict.Director}: {movieInfo.Director}";
            var country = string.IsNullOrEmpty(movieInfo.Country) ? null : $"{CurrentPhraseDict.Country}: {movieInfo.Country}";

            if (CurrentPhraseDict is EnPhraseDict)
                movieInfo.Title = movieInfo.OriginalTitle;

            var title = movieInfo.Title == movieInfo.OriginalTitle || movieInfo.OriginalTitle == null
                ? $"{CurrentPhraseDict.Title}: {movieInfo.Title}"
                : $"{CurrentPhraseDict.Title}: {movieInfo.Title} ({movieInfo.OriginalTitle})";

            var rating = string.Join(", ", movieInfo.Rating
                .Where(r => !string.IsNullOrEmpty(r.Value) && !r.Value.StartsWith("0"))
                .Select(r => $"{r.Key}: {r.Value}"));

            return string.Join("\r\n",
                       new string[] {title, year, director, country, rating}
                           .Where(p => !string.IsNullOrEmpty(p))) + "\r\n";
        }
    }
}