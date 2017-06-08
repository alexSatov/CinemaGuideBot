using System;
using System.Linq;
using System.Collections.Generic;

namespace CinemaGuideBot.Domain
{
    public struct MovieInfo
    {
        public const int DefaultYear = 1800;

        public string Title { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }
        public string OriginalTitle { get; set; }
        public Dictionary<string, string> Rating { get; set; }

        private int year;
        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1 || value > DateTime.Now.Year)
                    throw new ArgumentException($"Incorrect year ({value})");
                year = value;
            }
        }

        public override string ToString()
        {
            var year = Year == DefaultYear ? null : $"Год: {Year}";
            var director = string.IsNullOrEmpty(Director) ? null : $"Режиссер: {Director}";
            var country = string.IsNullOrEmpty(Country) ? null : $"Страна: {Country}";

            var title = string.IsNullOrEmpty(Title) 
                ? null 
                : Title == OriginalTitle || OriginalTitle == null 
                    ? $"Название: {Title}" 
                    : $"Название: {Title} ({OriginalTitle})";
            
            var rating = string.Join(", ", Rating
                .Where(r => !string.IsNullOrEmpty(r.Value))
                .Select(r => $"{r.Key}: {r.Value}"));

            return string.Join("\r\n", 
                new string[] {title, year, director, country, rating}
                .Where(p => !string.IsNullOrEmpty(p))) + "\r\n";
        }
    }
}